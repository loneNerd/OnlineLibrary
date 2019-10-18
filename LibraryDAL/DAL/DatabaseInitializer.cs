using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using System.Xml.Serialization;
using System.Web;
using LibraryDAL.Models;

namespace LibraryDAL
{
    class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        private const string _booksFileName = "Books.xml";

        protected override void Seed(DatabaseContext context)
        {
            Inventory newInventory;
            using (var stream = File.OpenRead(HttpContext.Current.Server.MapPath(_booksFileName)))
            {
                var serializer = new XmlSerializer(typeof(Inventory));
                newInventory = serializer.Deserialize(stream) as Inventory;
            }

            newInventory.BookList.ForEach(elem => context.Books.Add(elem));
            context.SaveChanges();
        }
    }
}
