using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryWebSite.Controllers;

namespace LibraryWebSite.Tests.Controllers
{
    [TestClass]
    public class LibrarianControllerTest
    {
        [TestMethod]
        public void Index()
        {
            LibrarianController controller = new LibrarianController();
            ActionResult result = controller.Index() as ActionResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AcceptOrder()
        {
            LibrarianController controller = new LibrarianController();
            ActionResult result1 = controller.AcceptOrder(null) as ActionResult;
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.AcceptOrder(1) as ActionResult;
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.AcceptOrder(10000) as ActionResult;
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public void DeclineOrder()
        {
            LibrarianController controller = new LibrarianController();
            ActionResult result1 = controller.DeclineOrder(null) as ActionResult;
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.DeclineOrder(1) as ActionResult;
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.DeclineOrder(10000) as ActionResult;
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public void CloseOrder()
        {
            LibrarianController controller = new LibrarianController();
            ActionResult result1 = controller.CloseOrder(null) as ActionResult;
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.CloseOrder(1) as ActionResult;
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.CloseOrder(10000) as ActionResult;
            Assert.IsNotNull(result3);
        }
    }
}
