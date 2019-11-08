using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Xml.Serialization;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LibraryDAL.Models;

namespace LibraryDAL
{
    /// <summary>
    /// This class initialize database when application start.
    /// </summary>
    class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        //Name of file which contains info about book
        private const string _booksFileName = "Books.xml";

        /// <summary>
        /// This method add data to database
        /// </summary>
        /// <param name="context">Context for this database</param>
        protected override void Seed(DatabaseContext context)
        {
            Inventory newInventory;

            //Filling data from xml file
            using (var stream = File.OpenRead(HttpContext.Current.Server.MapPath(_booksFileName)))
            {
                var serializer = new XmlSerializer(typeof(Inventory));
                newInventory = serializer.Deserialize(stream) as Inventory;
            }

            newInventory.BookList.ForEach(elem => context.Books.Add(elem));

            //Create new roles for database
            var roleManager = new RoleManager<DBRole, int>(new RoleStore<DBRole, int, DBUserRole>(context));

            var roles = new List<DBRole>()
            {
                new DBRole { Name = "Admin" },
                new DBRole { Name = "Reader" },
                new DBRole { Name = "Librarian" }
            };

            foreach (var role in roles)
                roleManager.Create(role);

            //Create new admin
            var UserManager = new UserManager<DBUser, int>(new UserStore<DBUser, DBRole, int, DBLogin, DBUserRole, DBClaim >(context));

            var admin = new DBUser
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com"
            };

            string userPWD = "P@ssw0rd";
            UserManager.Create(admin, userPWD);
            UserManager.AddToRole(admin.Id, "Admin");

            //Create new librarian
            var librarian = new DBUser
            {
                FirstName = "Jessica",
                LastName = "Tucker",
                UserName = "jess@gmail.com",
                Email = "jess@gmail.com",
            };

            //Create new reader
            var reader = new DBUser
            {
                FirstName = "Jack",
                LastName = "Doe",
                UserName = "jack@gmail.com",
                Email = "jack@gmail.com"
            };

            UserManager.Create(librarian, userPWD);
            UserManager.AddToRole(librarian.Id, "Librarian");

            UserManager.Create(reader, userPWD);
            UserManager.AddToRole(reader.Id, "Reader");

            context.SaveChanges();
        }
    }
}
