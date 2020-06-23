using System;
using System.Linq;
using System.Web.Mvc;
using NLog;
using LibraryWebSite.Models;
using LibraryDAL;
using LibraryDAL.Models;

namespace LibraryWebSite.Controllers
{
    public class AdminController : Controller
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        private IBookRepository _bookRepository;
        private IReaderRepository _readerRepository;
        private ILibrarianRepository _librarianRepository;

        public AdminController() { }

        public AdminController(IBookRepository bookRepository, IReaderRepository readerRepository, ILibrarianRepository librarianRepository)
        {
            BookRepository = bookRepository;
            ReaderRepository = readerRepository;
            LibrarianRepository = librarianRepository;
        }

        public IBookRepository BookRepository
        {
            get
            {
                if (_bookRepository == null)
                    _bookRepository = new BookRepository();

                return _bookRepository;
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
                if (_readerRepository == null)
                    _readerRepository = new ReaderRepository();

                return _readerRepository;
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
                if (_librarianRepository == null)
                    _librarianRepository = new LibrarianRepository();

                return _librarianRepository;
            }
            private set
            {
                _librarianRepository = value;
            }
        }

        //GET : /Admin/Index
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                if (!User.IsInRole("Admin"))
                {
                    _logger.Error($"Incorrect user {User.Identity.Name} redirection to admin controller");
                    return RedirectToAction("Index", "Home", new { page = 1 });
                }

                ViewBag.Books = BookRepository.GetBooks().ToList();
                ViewBag.Users = ReaderRepository.GetReaders().ToList();
                ViewBag.Librarians = LibrarianRepository.GetLibrarians().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View();
        }

        //GET : /Admin/EditBook
        [Authorize(Roles = "Admin")]
        public ActionResult EditBook(int? id)
        {
            try
            {
                if (id == null)
                    return Redirect(Request.Url.PathAndQuery);

                if (id == 0)
                    return View(new Book());

                var book = BookRepository.GetBookById((int)id);

                if (book == null)
                {
                    _logger.Error($"Book with id {id} never been exist in database");
                    return RedirectToAction("Index", "Admin");
                }

                return View(book);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Index", "Admin");
        }

        //POST : /Admin/EditBook
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditBook(Book book)
        {
            try
            {
                if (!DateTime.TryParse(book.PublicationDate, out DateTime testTime) || DateTime.Parse(book.PublicationDate) > DateTime.Now)
                    ModelState.AddModelError("PublicationDate", "Incorrect publication date");

                if (ModelState.IsValid)
                {
                    bool result;

                    if (book.BookID == 0)
                        result = BookRepository.AddNewBook(book);
                    else
                        result = BookRepository.EditBook(book);

                    if (result)
                        TempData["message"] = "Changes saved";
                    else
                        TempData["errorMessage"] = "Changes wasn't saved";

                    return RedirectToAction("Index", "Admin");
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                return View(book);
            }
            
            return View(book);
        }

        //GET : /Admin/DeleteBook
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteBook(int? id)
        {
            try
            {
                if (id == null)
                    return Redirect(Request.Url.PathAndQuery);

                if (BookRepository.DeleteBook((int)id))
                    TempData["message"] = "Book deleted";
                else
                    TempData["errorMessage"] = "Book wasn't deleted";
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Index", "Admin");
        }

        //GET : /Admin/ChangeReaderStatus
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeReaderStatus(int? id)
        {
            try
            {
                if (ReaderRepository.ChangeReaderStatus((int)id))
                    TempData["message"] = "Status has been changed";
                else
                    TempData["errorMessage"] = "Status hasn't been changed";
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Index", "Admin");
        }

        //GET : Admin/CreateLibrarian
        [Authorize(Roles = "Admin")]
        public ActionResult CreateLibrarian() => View(new RegisterViewModel());

        //GET : Admin/CreateLibrarian
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateLibrarian(RegisterViewModel librarian)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new DBUser
                    {
                        FirstName = librarian.FirstName,
                        LastName = librarian.LastName,
                        UserName = librarian.Email,
                        Email = librarian.Email
                    };

                    if (LibrarianRepository.AddNewLibrarian(user, librarian.Password))
                        TempData["message"] = "New librarian added";
                    else
                        TempData["errorMessage"] = "New librarian wasn't added";

                    return RedirectToAction("Index", "Admin");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View(librarian);
        }

        //GET : /Admin/DeleteLibrarian
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteLibrarian(int? id)
        {
            try
            {
                if (LibrarianRepository.DeleteLibrarian((int)id))
                    TempData["message"] = "Librarian deleted";
                else
                    TempData["errorMessage"] = "Librarian wasn't deleted";
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }
            
            return RedirectToAction("Index", "Admin");
        }
    }
}