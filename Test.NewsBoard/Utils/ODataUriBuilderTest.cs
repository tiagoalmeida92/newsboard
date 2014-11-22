using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBoard.Utils;

namespace Test.NewsBoard.Utils
{
    [TestClass]
    public class ODataUriBuilderTest
    {
        [TestMethod]
        public void TestSimple()
        {
            ODataQueryBuilder builder = ODataQueryBuilder.From("www.example.com")
                .Expand("Person")
                .OrderBy("Height", ODataQueryBuilder.Order.asc)
                .FilterAnd("Name eq Maria")
                .FilterAnd("Age eq 20");

            Assert.AreEqual("www.example.com?$expand=Person&$orderby=Height asc&$filter=Name eq Maria and Age eq 20",
                builder.Build());
        }
    }
}