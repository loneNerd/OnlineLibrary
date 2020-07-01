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
        private IDBRepository _dbRepository;

        public HomeController() { }

        public HomeController(IBookRepository bookRepository, IReaderRepository readerRepository, ILibrarianRepository librarianRepository, IPreOrderRepository preOrderRepository, IOrderRepository orderRepository, IDBRepository dBRepository)
        {
            BookRepository = bookRepository;
            ReaderRepository = readerRepository;
            LibrarianRepository = librarianRepository;
            PreOrderRepository = preOrderRepository;
            OrderRepository = orderRepository;
            DBRepository = dBRepository;
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

        public IDBRepository DBRepository
        {
            get
            {
                return _dbRepository ?? DependencyResolver.Current.GetService<IDBRepository>();
            }
            
            private set
            {
                _dbRepository = value;
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

                ViewBag.Title = "Catalog";

                if (page == null)
                    page = 1;

                IEnumerable<Book> books = BookRepository.GetBooks();

                if (books != null)
                {
                    BooksPageViewModels model = new BooksPageViewModels
                    {
                        Books = books,
                        Sort = _sort,
                        PageInfo = new PageInfo
                        {
                            CurrentPage = (int)page,
                            ItemsPerPage = _catalogPageCapacity,
                            TotalItems = books.Count(),
                        }
                    };

                    if (page > model.PageInfo.TotalPages)
                        return RedirectToAction("Index", "Home", new { page = 1 });

                    return View(model);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View(new BooksPageViewModels
            {
                Books = null,
                Sort = _sort,
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
            ReaderInfoViewModels readerInfo = new ReaderInfoViewModels();

            try
            {
                ViewBag.Title = "Info";
                readerInfo.Orders = OrderRepository.GetActiveOrders().Where(elem => elem.Reader.Id == User.Identity.GetUserId<int>());
                readerInfo.PreOrders = PreOrderRepository.GetActivePreOrders().Where(elem => elem.Reader.Id == User.Identity.GetUserId<int>());
                readerInfo.User = DBRepository.GetUserById(User.Identity.GetUserId<int>());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View(readerInfo);
        }

        //GET : Home/BookInfo
        public ActionResult BookInfo(int? id)
        {
            try
            {
                if (id == null)
                    return RedirectToAction("Index", "Home", new { page = 1 });

                ViewBag.Title = "Book Info";

                var book = BookRepository.GetBookById((int)id);

                if (book == null)
                    throw new ArgumentException("IDs on page and database don't match");

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
                ViewBag.Title = "Basket";
                return View(GetPreOrder().Select(elem => elem.Book));
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View(new List<Book>());
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
                    throw new ArgumentException("IDs on page and database don't match");

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
                    throw new ArgumentException("IDs on page and database don't match");

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

                ViewBag.Title = "Basket";
                ViewBag.Request = request;

                IEnumerable<Book> books = BookRepository.GetBooks()
                    .Where(elem => elem.Name.ToLower().Contains(request.ToLower()) || elem.Author.ToLower().Contains(request.ToLower()));

                if (books != null)
                {
                    BooksPageViewModels model = new BooksPageViewModels
                    {
                        Books = books,
                        Sort = _sort,
                        PageInfo = new PageInfo
                        {
                            CurrentPage = (int)page,
                            ItemsPerPage = _searchPageCapacity,
                            TotalItems = books.Count(),
                        }
                    };

                    if (page > model.PageInfo.TotalPages)
                        return RedirectToAction("SearchResult", "Home", new { request, page = 1 });

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View(new BooksPageViewModels
            {
                Books = null,
                Sort = _sort,
                PageInfo = new PageInfo
                {
                    CurrentPage = 1,
                    ItemsPerPage = _catalogPageCapacity,
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