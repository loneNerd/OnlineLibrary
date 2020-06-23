using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Models
{
    public class PreOrderRepository : DBRepository, IPreOrderRepository
    {
        /// <summary>
        /// Return active pre orders from database.
        /// </summary>
        /// <returns>Return IEnumerable<PreOrders> or null if not pre orders in database.</returns>
        public IEnumerable<PreOrder> GetActivePreOrders() => DatabaseContext.PreOrders.Where(elem => elem.Status == "Active");

        /// <summary>
        /// Method return pre order from database by id.
        /// </summary>
        /// <param name="id">ID of pre order.</param>
        /// <returns>If database contains pre order and it's active return PreOrder, else return false.</returns>
        public PreOrder GetActivePreOrderById(int id)
        {
            PreOrder preOrder = DatabaseContext.PreOrders.Find(id);

            if (preOrder == null || preOrder.Status != "Active")
                return null;

            return preOrder;
        }

        /// <summary>
        /// Method return pre order to database.
        /// </summary>
        /// <param name="preOrder">New pre order.</param>
        /// <returns>Return true in case of success, fasle if error</returns>
        public bool AddPreOrder(PreOrder preOrder)
        {
            if (preOrder == null)
                throw new ArgumentNullException($"Parametr {nameof(preOrder)} id null");

            if (preOrder.Book == null || preOrder.Status != "Active" || preOrder.Reader == null)
                return false;

            DatabaseContext.PreOrders.Add(preOrder);
            DatabaseContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Method disable pre order in database.
        /// </summary>
        /// <param name="id">ID of pre order.</param>
        /// <returns>True if pre order been disabled, false if not.</returns>
        public bool DisablePreOrder(int id)
        {
            PreOrder preOrder = DatabaseContext.PreOrders.Find(id);

            if (preOrder == null)
                return false;

            preOrder.Status = "Disable";

            DatabaseContext.SaveChanges();

            return true;
        }
    }
}
