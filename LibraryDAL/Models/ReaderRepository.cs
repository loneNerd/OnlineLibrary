using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryDAL.Models
{
    public class ReaderRepository : DBRepository, IReaderRepository
    {
        /// <summary>
        /// Method return readers in database.
        /// </summary>
        /// <returns>If database doesn't have any readers return null, else return IEnumerable<DBUser>.</returns>
        public IEnumerable<DBUser> GetReaders()
        {
            var role = DatabaseContext.Roles.FirstOrDefault(elem => elem.Name.Contains("Reader"));

            if (role == null)
                throw new ArgumentNullException($"Database doesn't contains role \"Reader\"");

            return DatabaseContext.Users.Where(elem => elem.Roles.FirstOrDefault(r => r.RoleId == role.Id) != null);
        }

        /// <summary>
        /// Method return reader from database by id.
        /// </summary>
        /// <param name="id">ID of reader.</param>
        /// <returns>If database contains reader return DBUser, else return null.</returns>
        public DBUser GetReaderById(int id) => DatabaseContext.Users.Find(id);

        /// <summary>
        /// Add new reader to database.
        /// </summary>
        /// <param name="reader">New reader.</param>
        /// <param name="password">Reader password.</param>
        /// <returns>True - if new reader added, false - if not.</returns>
        public bool AddNewReader(DBUser reader, string password)
        {
            if (reader == null)
                throw new ArgumentNullException($"Parametr {nameof(reader)} is null");

            var userManager = new UserManager<DBUser, int>(new UserStore<DBUser, DBRole, int, DBLogin, DBUserRole, DBClaim>(DatabaseContext));

            userManager.Create(reader, password);
            userManager.AddToRole(reader.Id, "Reader");
            DatabaseContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Method change reader status.
        /// </summary>
        /// <param name="id">ID of reader</param>
        /// <returns>True if status changed, false if not.</returns>
        public bool ChangeReaderStatus(int id)
        {
            DBUser reader = DatabaseContext.Users.Find(id);

            if (reader == null)
                return false;

            reader.IsBlock = !reader.IsBlock;
            DatabaseContext.SaveChanges();

            return true;
        }
    }
}
