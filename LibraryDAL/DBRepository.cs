using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LibraryDAL.Models;

namespace LibraryDAL
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks();
        Book GetBookById(int id);
        bool AddNewBook(Book book);
        bool EditBook(Book book);
        bool DeleteBook(int id);
    }

    public interface IReaderRepository
    {
        IEnumerable<DBUser> GetReaders();
        DBUser GetReaderById(int id);
        bool ChangeReaderStatus(int id);
    }

    public interface ILibrarianRepository
    {
        IEnumerable<DBUser> GetLibrarians();
        DBUser GetLibrarianById(int id);
        bool AddNewLibrarian(DBUser librarian, string password);
        bool DeleteLibrarian(int id);
    }

    public interface IPreOrderRepository
    {
        IEnumerable<PreOrder> GetActivePreOrders();
        PreOrder GetActivePreOrderById(int id);
        bool AddPreOrder(PreOrder preOrder);
        bool DisablePreOrder(int id);
    }

    public interface IOrderRepository
    {
        IEnumerable<Order> GetActiveOrders();
        Order GetActiveOrderById(int id);
        bool AddNewOrder(PreOrder preOrder, int id);
        bool CloseOrder(int id);
    }

    /// <summary>
    /// This is repository return data in database.
    /// </summary>
    public class DBRepository : IBookRepository, IReaderRepository, ILibrarianRepository, IPreOrderRepository, IOrderRepository
    {
        public ApplicationDbContext DatabaseContext { get; } = new ApplicationDbContext();

        #region Methods for book

        /// <summary>
        /// Return books from database.
        /// </summary>
        /// <returns>Return IEnumerable<Book> or null if not books in database.</returns>
        public IEnumerable<Book> GetBooks() => DatabaseContext.Books;

        /// <summary>
        /// Return book from database.
        /// </summary>
        /// <param name="id">Book id.</param>
        /// <returns>If database contains book method return book, else return null.</returns>
        public Book GetBookById(int id) => DatabaseContext.Books.Find(id);

        /// <summary>
        /// Add new book to database.
        /// </summary>
        /// <param name="book">New book.</param>
        /// <returns>True - if new book added, false - if not.</returns>
        public bool AddNewBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException($"Parametr {nameof(book)} is null");

            DatabaseContext.Books.Add(book);
            DatabaseContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Edit book in database.
        /// </summary>
        /// <param name="book">Book to edit.</param>
        /// <returns>True - if book edited, false - if not.</returns>
        public bool EditBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException($"Parametr {nameof(book)} is null");

            var newBook = GetBookById(book.BookID);

            if (newBook == null)
                return false;

            newBook.Author = book.Author;
            newBook.Description = book.Description;
            newBook.InStock = book.InStock;
            newBook.ISBN10 = book.ISBN10;
            newBook.Name = book.Name;
            newBook.Pages = book.Pages;
            newBook.PublicationDate = book.PublicationDate;
            newBook.Publisher = book.Publisher;

            DatabaseContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Delete book from database by book id.
        /// </summary>
        /// <param name="id"></param>
        /// /// <returns>True - if book delited, false - if not.</returns>
        public bool DeleteBook(int id)
        {
            var book = GetBookById(id);

            if (book == null)
                return false;

            DatabaseContext.Books.Attach(book);
            DatabaseContext.Books.Remove(book);
            DatabaseContext.SaveChanges();

            return true;
        }

        #endregion

        #region Methods for readers

        /// <summary>
        /// Method return readers in database.
        /// </summary>
        /// <returns>If database doesn't have any readers return null, else return IEnumerable<DBUser>.</returns>
        public IEnumerable<DBUser> GetReaders()
        {
            var role = DatabaseContext.Roles.FirstOrDefault(elem => elem.Name.Contains("Reader"));

            if (role == null)
                throw new ArgumentNullException($"Database doesn't contains role \"Reader\"");

            return DatabaseContext.Users.Where(elem => elem.Roles.FirstOrDefault(r => r.RoleId == role.Id) != null);
        }

        /// <summary>
        /// Method return reader from database by id.
        /// </summary>
        /// <param name="id">ID of reader.</param>
        /// <returns>If database contains reader return DBUser, else return null.</returns>
        public DBUser GetReaderById(int id) => DatabaseContext.Users.Find(id);

        /// <summary>
        /// Method change reader status.
        /// </summary>
        /// <param name="id">ID of reader</param>
        /// <returns>True if status changed, false if not.</returns>
        public bool ChangeReaderStatus(int id)
        {
            DBUser reader = DatabaseContext.Users.Find(id);

            if (reader == null)
                return false;

            reader.IsBlock = !reader.IsBlock;
            DatabaseContext.SaveChanges();

            return true;
        }

        #endregion

        #region Methods for librarian

        /// <summary>
        /// Method return librarians in database.
        /// </summary>
        /// <returns>If database doesn't have any librarians return null, else return IEnumerable<DBUser>.</returns>
        public IEnumerable<DBUser> GetLibrarians()
        {
            var role = DatabaseContext.Roles.FirstOrDefault(elem => elem.Name.Contains("Librarian"));

            if (role == null)
                throw new ArgumentNullException("Database doesn't contains role \"Librarian\"");

            return DatabaseContext.Users.Where(elem => elem.Roles.FirstOrDefault(r => r.RoleId == role.Id) != null && !elem.IsBlock);
        }

        /// <summary>
        /// Method return librarian from database by id.
        /// </summary>
        /// <param name="id">ID of librarian.</param>
        /// <returns>If database contains librarian return DBUser, else return null.</returns>
        public DBUser GetLibrarianById(int id) => DatabaseContext.Users.Find(id);

        /// <summary>
        /// Add new librarian to database.
        /// </summary>
        /// <param name="book">New librarian.</param>
        /// <param name="password">Librarian password.</param>
        /// <returns>True - if new librarian added, false - if not.</returns>
        public bool AddNewLibrarian(DBUser librarian, string password)
        {
            if (librarian == null)
                throw new ArgumentNullException($"Parametr {nameof(librarian)} is null");

            var userManager = new UserManager<DBUser, int>(new UserStore<DBUser, DBRole, int, DBLogin, DBUserRole, DBClaim>(DatabaseContext));

            userManager.Create(librarian, password);
            userManager.AddToRole(librarian.Id, "Librarian");
            DatabaseContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Method delete librarian from system and block librarian in database.
        /// </summary>
        /// <param name="id">ID of librarian</param>
        /// <returns>True - if new librarian added, false - if not.</returns>
        public bool DeleteLibrarian(int id)
        {
            var librarian = DatabaseContext.Users.FirstOrDefault(elem => elem.Id == id);

            if (librarian == null)
                return false;

            librarian.IsBlock = true;
            DatabaseContext.SaveChanges();

            return true;
        }

        #endregion

        #region Methods for pre orders

        /// <summary>
        /// Return active pre orders from database.
        /// </summary>
        /// <returns>Return IEnumerable<PreOrders> or null if not pre orders in database.</returns>
        public IEnumerable<PreOrder> GetActivePreOrders() => DatabaseContext.PreOrders.Where(elem => elem.Status == "Active");

        /// <summary>
        /// Method return pre order from database by id.
        /// </summary>
        /// <param name="id">ID of pre order.</param>
        /// <returns>If database contains pre order and it's active return PreOrder, else return false.</returns>
        public PreOrder GetActivePreOrderById(int id)
        {
            PreOrder preOrder = DatabaseContext.PreOrders.Find(id);

            if (preOrder == null || preOrder.Status != "Active")
                return null;

            return preOrder;
        }

        /// <summary>
        /// Method return pre order to database.
        /// </summary>
        /// <param name="preOrder">New pre order.</param>
        /// <returns>Return true in case of success, fasle if error</returns>
        public bool AddPreOrder(PreOrder preOrder)
        {
            if (preOrder == null)
                throw new ArgumentNullException($"Parametr {nameof(preOrder)} id null");

            if (preOrder.Book == null || preOrder.Status != "Active" || preOrder.Reader == null)
                return false;

            DatabaseContext.PreOrders.Add(preOrder);
            DatabaseContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Method disable pre order in database.
        /// </summary>
        /// <param name="id">ID of pre order.</param>
        /// <returns>True if pre order been disabled, false if not.</returns>
        public bool DisablePreOrder(int id)
        {
            PreOrder preOrder = DatabaseContext.PreOrders.Find(id);

            if (preOrder == null)
                return false;

            preOrder.Status = "Disable";

            DatabaseContext.SaveChanges();

            return true;
        }

        #endregion

        #region Methods for orders

        /// <summary>
        /// Method return active orders from database.
        /// </summary>
        /// <returns>If database doesn't contains any orders return null, else return IEnumerable<Order>.</returns>
        public IEnumerable<Order> GetActiveOrders() => DatabaseContext.Orders.Where(elem => elem.Status == "Active");

        /// <summary>
        /// Method return order from database by id.
        /// </summary>
        /// <param name="id">ID of order.</param>
        /// <returns>If database doesn't contains any active order return null, else return Order.</returns>
        public Order GetActiveOrderById(int id)
        {
            Order order = DatabaseContext.Orders.Find(id);

            if (order == null || order.Status != "Active")
                return null;

            return order;
        }

        /// <summary>
        /// Method add new order to database.
        /// </summary>
        /// <param name="preOrder">Pre order of book in database.</param>
        /// <param name="id">ID of librarian.</param>
        /// <returns>True if new oreder added to database, false if not.</returns>
        public bool AddNewOrder(PreOrder preOrder, int id)
        {
            if (preOrder == null)
                throw new ArgumentException($"Parametr {preOrder} is null");

            if (preOrder.Book == null || preOrder.Reader == null)
                return false;

            Book book = GetBookById(preOrder.Book.BookID);

            if (book == null)
                return false;

            DBUser librarian = GetLibrarianById(id);

            if (librarian == null)
                return false;

            if (DatabaseContext.Users.Find(preOrder.Reader.Id) == null)
                return false;

            book.InStock -= 1;

            DatabaseContext.Orders.Add(new Order
            {
                Book = book,
                Librarian = librarian,
                OrderDay = DateTime.UtcNow,
                Reader = preOrder.Reader,
                Status = "Active"
            });

            DatabaseContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Method close order.
        /// </summary>
        /// <param name="id">ID of order.</param>
        /// <returns>True if order been closed, false if not.</returns>
        public bool CloseOrder(int id)
        {
            Order order = DatabaseContext.Orders.Find(id);

            if (order == null)
                return false;

            Book book = DatabaseContext.Books.Find(order.Book.BookID);

            if (book == null)
                return false;

            book.InStock += 1;

            order.Status = "Close";
            order.CloseDay = DateTime.Now;

            DatabaseContext.SaveChanges();

            return true;
        }

        #endregion
        
        /// <summary>
        /// Method return any user in database by id.
        /// </summary>
        /// <param name="id">ID of user.</param>
        /// <returns>DBUser if database contains user, null if not.</returns>
        public DBUser GetUserById(int id) => DatabaseContext.Users.Find(id);

        /// <summary>
        /// Method return all users in database.
        /// </summary>=
        /// <returns>IEnumerable<DBUser> if database contains users, null if not.</returns>
        public IEnumerable<DBUser> GetUsers() => DatabaseContext.Users;
    }
}