using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryDAL.Models
{
    /// <summary>
    /// This class represent role with int key.
    /// </summary>
    public class DBRole : IdentityRole<int, DBUserRole> { }
}
