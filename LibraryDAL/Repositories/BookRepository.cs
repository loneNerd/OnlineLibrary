using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Models
{
    public class BookRepository : DBRepository, IBookRepository
    {
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
    }
}
