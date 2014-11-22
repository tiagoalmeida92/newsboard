using System.Security.Principal;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NewsBoard.Web.Controllers;

namespace Test.NewsBoard.Web
{
    [TestClass]
    public class IgnoredNewsSourcesTests
    {
        [TestMethod]
        public void TestIgnoredAll()
        {
            //Arrange
            string username = "admin";
            var ignored = new[] {4, 5, 6};
            var controller = new AccountController();
            //Mocking IIdentity
            var context = new Mock<ControllerContext>();
            var mockIdentity = new Mock<IPrincipal>();
            mockIdentity.Setup(x => x.Identity.Name).Returns(username);
            context.SetupGet(cctx => cctx.HttpContext.User).Returns(mockIdentity.Object);
            controller.ControllerContext = context.Object;

            //Act 
            //controller.UpdateIgnoredNewsSources(ignored.ToArray());

            //Assert
        }
    }
}