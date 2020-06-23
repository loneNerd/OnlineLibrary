using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Models
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks();
        Book GetBookById(int id);
        bool AddNewBook(Book book);
        bool EditBook(Book book);
        bool DeleteBook(int id);
    }
}
