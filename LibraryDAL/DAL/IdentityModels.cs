using Microsoft.AspNet.Identity.EntityFramework;
using LibraryDAL.Models;
using System.Data.Entity;

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

        public IDbSet<Book> Books { get; set; }
        public IDbSet<PreOrder> PreOrders { get; set; }
        public IDbSet<Order> Orders { get; set; }

        /// <summary>
        /// When database creating this method build comlumn and rows in database.
        /// </summary>
        /// <param name="modelBuilder">This standard model builder provided by entity framework.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(f => f.OrderDay).HasColumnType("datetime2");
            modelBuilder.Entity<Order>().Property(f => f.CloseDay).HasColumnType("datetime2");
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// This method create new database context
        /// </summary>
        /// <returns>Returns reference to new database context for loggin system</returns>
        public static ApplicationDbContext Create() => new ApplicationDbContext();
    }
}