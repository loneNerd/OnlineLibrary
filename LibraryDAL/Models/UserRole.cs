using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryDAL.Models
{
    /// <summary>
    /// This class represent one to one dependency between role and user.
    /// </summary>
    public class DBUserRole : IdentityUserRole<int> { }
}
