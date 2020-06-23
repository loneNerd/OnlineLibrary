using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LibraryDAL.Models;

namespace LibraryDAL
{
    /// <summary>
    /// This is repository return data in database.
    /// </summary>
    public class DBRepository : IDBRepository
    {
        public static ApplicationDbContext DatabaseContext { get; } = new ApplicationDbContext();

        /// <summary>
        /// Method return any user in database by id.
        /// </summary>
        /// <param name="id">ID of user.</param>
        /// <returns>DBUser if database contains user, null if not.</returns>
        public DBUser GetUserById(int id) => DatabaseContext.Users.Find(id);

        /// <summary>
        /// Method return all users in database.
        /// </summary>=
        /// <returns>IEnumerable<DBUser> if database contains users, null if not.</returns>
        public IEnumerable<DBUser> GetUsers() => DatabaseContext.Users;
    }
}