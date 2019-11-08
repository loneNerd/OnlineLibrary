using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryDAL.Models
{
    /// <summary>
    /// This class represent User with int key and add new field to standart implementation.
    /// </summary>
    public class DBUser : IdentityUser<int, DBLogin, DBUserRole, DBClaim>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlock { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<DBUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
