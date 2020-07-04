using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Models
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetActiveOrders();
        Order GetActiveOrderById(int id);
        bool AddNewOrder(PreOrder preOrder, int id);
        bool CloseOrder(int id);
    }
}
