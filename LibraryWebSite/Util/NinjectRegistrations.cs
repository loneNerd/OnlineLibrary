using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryDAL.Models;
using Ninject.Modules;

namespace LibraryWebSite.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IDBRepository>().To<DBRepository>();
            Bind<IBookRepository>().To<BookRepository>();
            Bind<IReaderRepository>().To<ReaderRepository>();
            Bind<ILibrarianRepository>().To<LibrarianRepository>();
            Bind<IPreOrderRepository>().To<PreOrderRepository>();
            Bind<IOrderRepository>().To<OrderRepository>();
        }
    }
}