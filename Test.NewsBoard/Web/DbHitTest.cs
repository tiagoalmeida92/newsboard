using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBoard.Persistence;

namespace Test.NewsBoard.Web
{
    [TestClass]
    public class DbHitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var db = new NewsDb();
            Assert.AreNotEqual(-1, db.NewsItems.Count());
        }
    }
}