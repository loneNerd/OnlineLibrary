using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LibraryDAL.Models;

namespace LibraryDAL
{
    public class DataFromDB
    {
        private static readonly DatabaseContext _databaseContext = new DatabaseContext();

        public static void InitializeDB()
        {
            Database.SetInitializer(new DatabaseInitializer());
            _databaseContext.Database.Initialize(true);
            _databaseContext.Database.CreateIfNotExists();
        }

        public static List<Book> GetBooks() => _databaseContext.Books.ToList();
    }
}
