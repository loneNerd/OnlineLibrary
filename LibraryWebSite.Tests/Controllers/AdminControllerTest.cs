using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryWebSite.Controllers;
using LibraryDAL.Models;

namespace LibraryWebSite.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTest
    {
        [TestMethod]
        public void Index()
        {
            AdminController controller = new AdminController();
            ActionResult result = controller.Index() as ActionResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditBook()
        {
            AdminController controller = new AdminController();
            ActionResult result1 = controller.EditBook(1) as ActionResult;
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.EditBook(100000) as ActionResult;
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.EditBook(new Book
            {
                Author = "Test Author",
                Description = "Test Description",
                InStock = 5,
                ISBN10 = "829349238",
                Name = "Test Name",
                Pages = 453,
                PublicationDate = DateTime.Now.ToString("dd MMMM yyyy"),
                Publisher = "Test Publisher"
            });
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public void DeleteBook()
        {
            AdminController controller = new AdminController();
            ActionResult result1 = controller.DeleteBook(null);
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.DeleteBook(1);
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.DeleteBook(1000000);
            Assert.IsNotNull(result3);

        }

        [TestMethod]
        public void ChangeReaderStatus()
        {
            AdminController controller = new AdminController();
            ActionResult result1 = controller.ChangeReaderStatus(null);
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.ChangeReaderStatus(1);
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.ChangeReaderStatus(100000);
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public void CreateLibrarian()
        {
            AdminController controller = new AdminController();
            ActionResult result1 = controller.CreateLibrarian();
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.CreateLibrarian(new Models.RegisterViewModel
            {
                Email = "test@gmail.com",
                LastName = "TestLName",
                FirstName = "TestFName",
                Password = "P@ssw0rd",
                ConfirmPassword = "P@ssw0rd"
            });
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void DeleteLibrarian()
        {
            AdminController controller = new AdminController();
            ActionResult result1 = controller.DeleteLibrarian(null);
            Assert.IsNotNull(result1);
            ActionResult result2 = controller.DeleteLibrarian(1);
            Assert.IsNotNull(result2);
            ActionResult result3 = controller.DeleteLibrarian(1000000);
            Assert.IsNotNull(result3);
        }
    }
}
