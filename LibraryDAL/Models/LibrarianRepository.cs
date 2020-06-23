using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryDAL.Models
{
    public class LibrarianRepository : DBRepository, ILibrarianRepository
    {
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
    }
}
