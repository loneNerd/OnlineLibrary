using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWebSite.Models
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get
            {
                return TotalItems / ItemsPerPage;
            }
        }
    }
}