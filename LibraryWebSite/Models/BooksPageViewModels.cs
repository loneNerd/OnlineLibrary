using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryDAL;
using LibraryDAL.Models;

namespace LibraryWebSite.Models
{
    public class BooksPageViewModels
    {
        public IEnumerable<Book> Books { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}