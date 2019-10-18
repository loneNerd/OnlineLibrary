using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LibraryDAL.Models;

namespace LibraryDAL
{
    class DatabaseContext : ApplicationDbContext
    {   
        public IDbSet<Book> Books { get; set; }
    }
}
