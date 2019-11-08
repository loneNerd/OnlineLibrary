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

                List<PreOrder> preOrders = DBRepository.GetActivePreOrders().ToList();
                preOrders.Sort((elem1, elem2) => (elem1.Reader.FirstName + elem1.Reader.LastName).CompareTo((elem2.Reader.FirstName + elem2.Reader.LastName)));
                ViewBag.PreOrders = preOrders;
                ViewBag.Orders = DBRepository.GetActiveOrders().ToList();
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

                PreOrder preOrder = DBRepository.GetActivePreOrderById((int)id);

                if (preOrder == null)
                    return RedirectToAction("Index", "Librarian");

                DBRepository.DisablePreOrder((int)id);

                DBRepository.AddNewOrder(preOrder, User.Identity.GetUserId<int>());
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

                PreOrder preOrder = DBRepository.GetActivePreOrderById((int)id);

                if (preOrder == null)
                    return RedirectToAction("Index", "Librarian");

                DBRepository.DisablePreOrder((int)id);

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

                Order order = DBRepository.GetActiveOrderById((int)id);

                if (order == null)
                    return RedirectToAction("Index", "Librarian");
                
                DBRepository.CloseOrder((int)id);

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