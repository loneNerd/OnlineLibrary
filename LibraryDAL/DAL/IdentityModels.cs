using Microsoft.AspNet.Identity.EntityFramework;
using LibraryDAL.Models;

namespace LibraryDAL
{
    /// <summary>
    /// This class present for database context for loggin system.
    /// Class inherit from IdentityDBContext.
    /// Main change compare to default implementation is now ID have type of int.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<DBUser, DBRole, int, DBLogin, DBUserRole, DBClaim>
    {
        public ApplicationDbContext() : base("DataBaseContext") { }

        /// <summary>
        /// This method create new database context
        /// </summary>
        /// <returns>Returns reference to new database context for loggin system</returns>
        public static ApplicationDbContext Create() => new ApplicationDbContext();
    }
}