using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Models
{
    public interface IPreOrderRepository
    {
        IEnumerable<PreOrder> GetActivePreOrders();
        PreOrder GetActivePreOrderById(int id);
        bool AddPreOrder(PreOrder preOrder);
        bool DisablePreOrder(int id);
    }
}
