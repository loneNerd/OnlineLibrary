using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NLog;
using LibraryWebSite.Models;
using LibraryDAL;
using LibraryDAL.Models;

namespace LibraryWebSite.Controllers
{
    public class HomeController : Controller
    {
        private const int _catalogPageCapacity = 24;
        private const int _searchPageCapacity = 10;
        private static string _sort = "name";
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private IBookRepository _bookRepository;
        private IReaderRepository _readerRepository;
        private ILibrarianRepository _librarianRepository;
        private IPreOrderRepository _preOrderRepository;
        private IOrderRepository _orderRepository;

        public HomeController() { }

        public HomeController(IBookRepository bookRepository, IReaderRepository readerRepository, ILibrarianRepository librarianRepository, IPreOrderRepository preOrderRepository, IOrderRepository orderRepository)
        {
            BookRepository = bookRepository;
            ReaderRepository = readerRepository;
            LibrarianRepository = librarianRepository;
            PreOrderRepository = preOrderRepository;
            OrderRepository = orderRepository;
        }

        public IBookRepository BookRepository
        {
            get
            {
                return _bookRepository ?? DependencyResolver.Current.GetService<IBookRepository>();
            }
            private set 
            { 
                _bookRepository = value; 
            }
        }

        public IReaderRepository ReaderRepository
        {
            get
            {
                return _readerRepository ?? DependencyResolver.Current.GetService<IReaderRepository>();
            }
            private set
            {
                _readerRepository = value;
            }
        }

        public ILibrarianRepository LibrarianRepository
        {
            get
            {
                return _librarianRepository ?? DependencyResolver.Current.GetService<ILibrarianRepository>();
            }
            private set
            {
                _librarianRepository = value;
            }
        }

        public IPreOrderRepository PreOrderRepository
        {
            get
            {
                return _preOrderRepository ?? DependencyResolver.Current.GetService<IPreOrderRepository>();
            }
            private set
            {
                _preOrderRepository = value;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                return _orderRepository ?? DependencyResolver.Current.GetService<IOrderRepository>();
            }
            private set
            {
                _orderRepository = value;
            }
        }

        //GET : Home/Index
        public ActionResult Index(int? page = 1)
        {
            try
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Admin");
                else if (User.IsInRole("Librarian"))
                    return RedirectToAction("Index", "Librarian");
                
                if (page == null)
                    page = 1;

                List<Book> books;

                switch (_sort)
                {
                    case "author":
                        books = BookRepository.GetBooks().OrderBy(book => book.Author).ToList();
                        break;
                    case "publisher":
                        books = BookRepository.GetBooks().OrderBy(book => book.Publisher).ToList();
                        break;
                    case "publicationDate":
                        books = BookRepository.GetBooks().OrderBy(book => book.PublicationDate).ToList();
                        books.Sort((book1, book2) => DateTime.Parse(book1.PublicationDate).CompareTo(DateTime.Parse(book2.PublicationDate)));
                        break;
                    case "name":
                    default:
                        books = BookRepository.GetBooks().OrderBy(book => book.Name).ToList();
                        break;
                }

                int start = (((int)page - 1) * _catalogPageCapacity);
                int end = books.Count - start > _catalogPageCapacity ? _catalogPageCapacity : books.Count - start;

                BooksPageViewModels model = new BooksPageViewModels
                {
                    Books = books.GetRange(start, end),
                    PageInfo = new PageInfo
                    {
                        CurrentPage = (int)page,
                        ItemsPerPage = _catalogPageCapacity,
                        TotalItems = books.Count,
                    }
                };

                return View(model);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View(new BooksPageViewModels
            {
                Books = new List<Book>(),
                PageInfo = new PageInfo
                {
                    CurrentPage = 1,
                    ItemsPerPage = _catalogPageCapacity,
                    TotalItems = 0
                }
            });
        }

        //GET : Home/ChangeSort
        public ActionResult ChangeSort(string sort, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(sort))
                _sort = "name";
            else
                _sort = sort;

            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index", "Home", new { page = 1 });

            return Redirect(returnUrl);
        }

        //GET : Home/ReaderInfo
        [Authorize]
        public ActionResult ReaderInfo()
        {
            try
            {
                ViewBag.PreOrders = PreOrderRepository.GetActivePreOrders().Where(elem => elem.Reader.Id == User.Identity.GetUserId<int>()).ToList();
                ViewBag.Orders = OrderRepository.GetActiveOrders().Where(elem => elem.Reader.Id == User.Identity.GetUserId<int>()).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View();
        }

        //GET : Home/BookInfo
        public ActionResult BookInfo(int? id)
        {
            try
            {
                if (id == null)
                    return RedirectToAction("Index", "Home", new { page = 1 });

                var book = BookRepository.GetBookById((int)id);

                if (book == null)
                    return RedirectToAction("Index", "Home", new { page = 1 });

                return View(book);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Index", "Home", new { page = 1 });
        }

        //GET : Home/Basket
        [Authorize]
        public ActionResult Basket()
        {
            try
            {
                ViewBag.Books = GetPreOrder().Select(elem => elem.Book).ToList();
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View();
        }

        //GET : Home/Basket
        [Authorize]
        public ActionResult AddToBasket(int? id, string returnUrl)
        {
            try
            {
                if (id == null)
                    return RedirectToAction("Index", "Home", new { page = 1 });

                Book book = BookRepository.GetBookById((int)id);

                if (book == null)
                    return RedirectToAction("Index", "Home", new { page = 1 });

                GetPreOrder().Add(new PreOrder
                {
                    Book = book,
                    Status = "Active",
                    Reader = ReaderRepository.GetReaderById(User.Identity.GetUserId<int>())
                });

                return RedirectToAction("Basket", "Home");
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Index", "Home", new { page = 1 });
        }

        //GET : /Home/Basket
        [Authorize]
        public ActionResult RemoveFromBasket(int? id)
        {
            try
            {
                if (id == null)
                    return RedirectToAction("Basket", "Home");

                PreOrder book = GetPreOrder().Find(elem => elem.Book.BookID == id);

                if (book == null)
                    return RedirectToAction("Basket", "Home");

                GetPreOrder().Remove(book);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Basket", "Home");
        }

        //GET : Home/Index
        [Authorize]
        public ActionResult MakeOrder()
        {
            try
            {
                if (GetPreOrder() == null || GetPreOrder().Count == 0)
                    return RedirectToAction("Index", "Home", new { page = 1 });

                GetPreOrder().ForEach(elem => PreOrderRepository.AddPreOrder(new PreOrder
                {
                    Book = elem.Book,
                    Reader = elem.Reader,
                    Status = elem.Status
                }));

                GetPreOrder().Clear();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Index", "Home", new { page = 1 });
        }

        //GET : /Home/SearchResult
        public ActionResult SearchResult(string request, int? page = 1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request) || page == null)
                    return RedirectToAction("Index", "Home", new { page = 1 });

                ViewBag.Request = request;

                List<Book> books = BookRepository.GetBooks()
                    .Where(elem => elem.Name.ToLower().Contains(request.ToLower()) || elem.Author.ToLower().Contains(request.ToLower())).ToList();

                if (books.Count / _searchPageCapacity < page)
                    return RedirectToAction("Index", "Home", new { page = 1 });

                int start = (((int)page - 1) * _searchPageCapacity);
                int end = books.Count - start > _searchPageCapacity ? _searchPageCapacity : books.Count - start;

                BooksPageViewModels model = new BooksPageViewModels
                {
                    Books = books.GetRange(start, end),
                    PageInfo = new PageInfo
                    {
                        CurrentPage = (int)page,
                        ItemsPerPage = _searchPageCapacity,
                        TotalItems = books.Count,
                    }
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View(new BooksPageViewModels
            {
                Books = new List<Book>(),
                PageInfo = new PageInfo
                {
                    CurrentPage = 1,
                    ItemsPerPage = _searchPageCapacity,
                    TotalItems = 0
                }
            });
        }

        private List<PreOrder> GetPreOrder()
        {
            List<PreOrder> preOrder = (List<PreOrder>)Session["PreOrder"];

            if (preOrder == null)
            {
                preOrder = new List<PreOrder>();
                Session["PreOrder"] = preOrder;
            }

            return preOrder;
        }
    }
}