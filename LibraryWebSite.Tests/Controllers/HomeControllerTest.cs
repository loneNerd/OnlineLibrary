using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryWebSite.Controllers;

namespace LibraryWebSite.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index(1) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ChangeSort()
        {
            HomeController controller = new HomeController();
            ActionResult result = controller.ChangeSort("author", "Home/Index/?page=1") as ActionResult;
            Assert.IsNotNull(result);
            ActionResult result2 = controller.ChangeSort("publisher", "") as ActionResult;
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.ChangeSort("", "Home/Index/?page=1") as ActionResult;
            Assert.IsNotNull(result3);
            ActionResult result4 = controller.ChangeSort(null, null) as ActionResult;
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void ReaderInfo()
        {
            HomeController controller = new HomeController();
            ActionResult result = controller.ReaderInfo() as ActionResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void BookInfo()
        {
            HomeController controller = new HomeController();
            ActionResult result1 = controller.BookInfo(null) as ActionResult;
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.BookInfo(100000) as ActionResult;
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.BookInfo(0) as ActionResult;
            Assert.IsNotNull(result3);
            ActionResult result4 = controller.BookInfo(1) as ActionResult;
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void Basket()
        {
            HomeController controller = new HomeController();
            ActionResult result = controller.Basket() as ActionResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddToBasket()
        {
            HomeController controller = new HomeController();
            ActionResult result1 = controller.AddToBasket(null, null) as ActionResult;
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.AddToBasket(null, "Home/Index/?page=1") as ActionResult;
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.AddToBasket(0, "Home/Index/?page=1") as ActionResult;
            Assert.IsNotNull(result3);
            ActionResult result4 = controller.AddToBasket(100000, "Home/Index/?page=1") as ActionResult;
            Assert.IsNotNull(result4);
            ActionResult result5 = controller.AddToBasket(1, "Home/Index/?page=1") as ActionResult;
            Assert.IsNotNull(result5);
        }

        [TestMethod]
        public void RemoveFromBasket()
        {
            HomeController controller = new HomeController();
            ActionResult result1 = controller.RemoveFromBasket(null) as ActionResult;
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.RemoveFromBasket(0) as ActionResult;
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.RemoveFromBasket(100000) as ActionResult;
            Assert.IsNotNull(result3);
            ActionResult result4 = controller.RemoveFromBasket(1) as ActionResult;
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void MakeOrder()
        {
            HomeController controller = new HomeController();
            ActionResult result = controller.MakeOrder() as ActionResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SearchResult()
        {
            HomeController controller = new HomeController();
            ActionResult result1 = controller.SearchResult(null, null) as ActionResult;
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.SearchResult("of", 1) as ActionResult;
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.SearchResult("of", 10000) as ActionResult;
            Assert.IsNotNull(result3);
        }
    }
}
