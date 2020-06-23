using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Models
{
    public class OrderRepository : DBRepository, IOrderRepository
    {
        /// <summary>
        /// Method return active orders from database.
        /// </summary>
        /// <returns>If database doesn't contains any orders return null, else return IEnumerable<Order>.</returns>
        public IEnumerable<Order> GetActiveOrders() => DatabaseContext.Orders.Where(elem => elem.Status == "Active");

        /// <summary>
        /// Method return order from database by id.
        /// </summary>
        /// <param name="id">ID of order.</param>
        /// <returns>If database doesn't contains any active order return null, else return Order.</returns>
        public Order GetActiveOrderById(int id)
        {
            Order order = DatabaseContext.Orders.Find(id);

            if (order == null || order.Status != "Active")
                return null;

            return order;
        }

        /// <summary>
        /// Method add new order to database.
        /// </summary>
        /// <param name="preOrder">Pre order of book in database.</param>
        /// <param name="id">ID of librarian.</param>
        /// <returns>True if new oreder added to database, false if not.</returns>
        public bool AddNewOrder(PreOrder preOrder, int id)
        {
            if (preOrder == null)
                throw new ArgumentException($"Parametr {preOrder} is null");

            if (preOrder.Book == null || preOrder.Reader == null)
                return false;

            Book book = DatabaseContext.Books.Find(preOrder.Book.BookID);

            if (book == null)
                return false;

            DBUser librarian = DatabaseContext.Users.Find(id);

            if (librarian == null)
                return false;

            if (DatabaseContext.Users.Find(preOrder.Reader.Id) == null)
                return false;

            book.InStock -= 1;

            DatabaseContext.Orders.Add(new Order
            {
                Book = book,
                Librarian = librarian,
                OrderDay = DateTime.UtcNow,
                Reader = preOrder.Reader,
                Status = "Active"
            });

            DatabaseContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Method close order.
        /// </summary>
        /// <param name="id">ID of order.</param>
        /// <returns>True if order been closed, false if not.</returns>
        public bool CloseOrder(int id)
        {
            Order order = DatabaseContext.Orders.Find(id);

            if (order == null)
                return false;

            Book book = DatabaseContext.Books.Find(order.Book.BookID);

            if (book == null)
                return false;

            book.InStock += 1;

            order.Status = "Close";
            order.CloseDay = DateTime.Now;

            DatabaseContext.SaveChanges();

            return true;
        }
    }
}
