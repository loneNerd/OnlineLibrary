using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryDAL.Models;

namespace LibraryWebSite.Models
{
    public class AdminViewModels
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<DBUser> Readers { get; set; }
        public IEnumerable<DBUser> Librarians { get; set; }
    }
}