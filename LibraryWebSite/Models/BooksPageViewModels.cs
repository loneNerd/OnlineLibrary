using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using LibraryDAL;
using LibraryDAL.Models;

namespace LibraryWebSite.Models
{
    public class BooksPageViewModels
    {
        private IEnumerable<Book> _books;

        public IEnumerable<Book> Books
        {
            get
            {
                if (_books == null)
                    return new List<Book>();

                int start = (PageInfo.CurrentPage - 1) * PageInfo.ItemsPerPage;
                int end = _books.Count() - start > PageInfo.ItemsPerPage ? PageInfo.ItemsPerPage : _books.Count() - start;

                List<Book> books;

                switch (Sort)
                {
                    case "author":
                        books = new List<Book>(_books.OrderBy(book => book.Author));
                        break;
                    case "publisher":
                        books = new List<Book>(_books.OrderBy(book => book.Publisher));
                        break;
                    case "publicationDate":
                        books = new List<Book>(_books.OrderBy(book => book.PublicationDate));
                        books.Sort((book1, book2) => DateTime.Parse(book1.PublicationDate).CompareTo(DateTime.Parse(book2.PublicationDate)));
                        break;
                    case "name":
                    default:
                        books = new List<Book>(_books.OrderBy(book => book.Name));
                        break;
                }

                return books.GetRange(start, end);
            }

            set { _books = value; }
        }

        public PageInfo PageInfo { get; set; }

        public string Sort { get; set; }
    }
}