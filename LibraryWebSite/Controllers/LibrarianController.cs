using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NLog;
using LibraryDAL;
using LibraryDAL.Models;

namespace LibraryWebSite.Controllers
{
    public class LibrarianController : Controller
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        private IPreOrderRepository _preOrderRepository;
        private IOrderRepository _orderRepository;

        public LibrarianController() { }

        public LibrarianController(IPreOrderRepository preOrderRepository, IOrderRepository orderRepository)
        {
            PreOrderRepository = preOrderRepository;
            OrderRepository = orderRepository;
        }

        public IPreOrderRepository PreOrderRepository
        {
            get
            {
                return _preOrderRepository ?? DependencyResolver.Current.GetService<IPreOrderRepository>();
            }
            private set
            {
                _preOrderRepository = value;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                return _orderRepository ?? DependencyResolver.Current.GetService<IOrderRepository>();
            }
            private set
            {
                _orderRepository = value;
            }
        }

        //GET : /Librarian/Index
        [Authorize(Roles = "Librarian")]
        public ActionResult Index()
        {
            try
            {
                if (!User.IsInRole("Librarian"))
                {
                    _logger.Error($"Incorrect user {User.Identity.Name} redirection to admin controller");
                    return RedirectToAction("Index", "Home", new { page = 1 });
                }

                List<PreOrder> preOrders = PreOrderRepository.GetActivePreOrders().ToList();
                preOrders.Sort((elem1, elem2) => (elem1.Reader.FirstName + elem1.Reader.LastName).CompareTo((elem2.Reader.FirstName + elem2.Reader.LastName)));
                ViewBag.PreOrders = preOrders;
                ViewBag.Orders = OrderRepository.GetActiveOrders().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return View();
        }

        //GET : /Librarian/Index
        [Authorize(Roles = "Librarian")]
        public ActionResult AcceptOrder(int? id)
        {
            try
            {
                if (id == null)
                    return RedirectToAction("Index", "Librarian");

                PreOrder preOrder = PreOrderRepository.GetActivePreOrderById((int)id);

                if (preOrder == null)
                    return RedirectToAction("Index", "Librarian");

                PreOrderRepository.DisablePreOrder((int)id);

                OrderRepository.AddNewOrder(preOrder, User.Identity.GetUserId<int>());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Index", "Librarian");
        }

        //GET : /Librarian/Index
        [Authorize(Roles = "Librarian")]
        public ActionResult DeclineOrder(int? id)
        {
            try
            {
                if (id == null)
                    return RedirectToAction("Index", "Librarian");

                PreOrder preOrder = PreOrderRepository.GetActivePreOrderById((int)id);

                if (preOrder == null)
                    return RedirectToAction("Index", "Librarian");

                PreOrderRepository.DisablePreOrder((int)id);

                return RedirectToAction("Index", "Librarian");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Index", "Librarian");
        }

        //GET : /Librarian/Index
        [Authorize(Roles = "Librarian")]
        public ActionResult CloseOrder(int? id)
        {
            try
            {
                if (id == null)
                    return RedirectToAction("Index", "Librarian");

                Order order = OrderRepository.GetActiveOrderById((int)id);

                if (order == null)
                    return RedirectToAction("Index", "Librarian");

                OrderRepository.CloseOrder((int)id);

                return RedirectToAction("Index", "Librarian");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return RedirectToAction("Index", "Librarian");
        }
    }
}