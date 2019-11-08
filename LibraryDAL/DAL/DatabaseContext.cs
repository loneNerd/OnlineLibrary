using System.Data.Entity;
using LibraryDAL.Models;

namespace LibraryDAL
{
    /// <summary>
    /// This class complements standart database context, which implemented by standart identity manager.
    /// This class add books, pre orders and orders.
    /// </summary>
    public class DatabaseContext : ApplicationDbContext
    {
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
    }
}
