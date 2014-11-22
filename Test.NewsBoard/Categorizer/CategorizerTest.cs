//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NewsBoard.Categorizer;
//using NewsBoard.Indexer;
//using NewsBoard.Model;
//using NewsBoard.Persistence;

//namespace Test.NewsBoard.Categorizer
//{
//    /// <summary>
//    /// To run all tests successfully need to have news scraped and indexed
//    /// </summary>
//    [TestClass]
//    public class CategorizerTest
//    {
//        [TestMethod]
//        public void TestAutomaticCategorizer()
//        {
//            using (var newsDb = new NewsDb())
//            {
//                IList<NewsCategory> newsCategories = newsDb.NewsCategories.ToList();
//                IIndexer<NewsItem> indexer = new WordsDecorator<NewsItem>(
//                    new SearchDecorator(
//                        new NewsIndexer()));
//                var categorize = new Categorize(indexer);
//                Assert.IsTrue(categorize.Automatic(newsCategories)!=0);
//            }
//        }

//    }
//}