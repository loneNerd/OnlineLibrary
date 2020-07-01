using System;
using System.Collections.Generic;
using LibraryDAL.Models;

namespace LibraryWebSite.Models
{
    public class LibrarianViewModels
    {
        public IEnumerable<PreOrder> PreOrders { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}