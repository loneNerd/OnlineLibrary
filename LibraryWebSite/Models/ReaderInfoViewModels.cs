using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryDAL.Models;

namespace LibraryWebSite.Models
{
    public class ReaderInfoViewModels
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<PreOrder> PreOrders { get; set; }
        public DBUser User { get; set; }
    }
}