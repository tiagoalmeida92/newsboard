using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Lucene.Net.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBoard.Constants;
using NewsBoard.Indexer;
using NewsBoard.Indexer.Model;
using NewsBoard.Model;
using NewsBoard.Model.Categories;
using NewsBoard.Persistence;

namespace Test.NewsBoard.Indexer
{
    /// <summary>
    /// To run all tests successfully need to have news scraped and indexed
    /// </summary>
    [TestClass]
    public class IndexerTest
    {
        
        [TestMethod]
        public void TestIndexCategories()
        {
            using (var ctx = new NewsDb())
            {
                var indexer = new WordsDecorator<NewsItem>(new NewsIndexer());

                //Test when we had the dictionary of word,frequency,number of docs
                //var dict = new Dictionary<Tuple<String, String, int>, Dictionary<String, FrequencyAndDocsCount>>();

                //Uncomment when index directory is empty
                //indexer.Index(ctx.NewsItems.ToList(),true);
                int newsTotalCount = 0;
                var finaList = new List<string>();
                ctx.NewsCategories.ForEachAsync(category =>
                {
                    finaList = indexer.GetTopWordsFilter(Constants.ARTICLE_FIELD, Constants.CATEGORY_FIELD, category.Name, out newsTotalCount);
                    Assert.IsTrue(finaList.Count != 0 && newsTotalCount != 0);
                    newsTotalCount = 0;
                    //Test when we had the dictionary of word,frequency,number of docs
                    //dict.Add(new Tuple<string, string,int>( tuple.Item1,tuple.Item2,newsTotalCount), topWords);
                });
            }
        }

        [TestMethod]
        public void TestSearchForWords()
        {
            using (var ctx = new NewsDb())
            {
                var indexer = new SearchDecorator(new NewsIndexer());
                var dict = new Dictionary<Tuple<String, String, int>, Dictionary<String, FrequencyAndDocsCount>>();
                //Uncomment when index directory is empty
                //indexer.Index(ctx.NewsItems.ToList(), true);
                List<String> links = indexer.Search("benfica").ToList();

                Assert.IsTrue(links.Count != 0);
            }
        }

        [TestMethod]
        public void TestIndexUndifferentiatedCategories()
        {
            using (var ctx = new NewsDb())
            {
                NewsItem item =
                    ctx.NewsItems.ToList()
                        .First(newsItem => newsItem.CategoryName.Equals(Constants.DEFAULTMANUALCATEGORY));
                var undifferentiated = new UndifferentiatedCategory
                {
                    Category = "Nacional",
                    Link = item.Link,
                    TopSubjectsWords = "Porto Portugal Festa S.João"
                };
                var manualWordsIndexer = new WordsDecorator<UndifferentiatedCategory>(new ManualWordsIndexer());
                manualWordsIndexer.Index(undifferentiated, false);
                int newsTotalCount = 0;
                List<string> finaList = manualWordsIndexer.GetTopWordsFilter(Constants.ADMINSUBJECTS_FIELD,
                    Constants.LINK_FIELD, item.Link, out newsTotalCount);
                Assert.IsNotNull(finaList);
            }
        }

        [TestMethod]
        public void TestUpdateIndexedNewsCategories()
        {
            const string categoryAux = "Desporto";
            NewsItem newsitemFinal = null;
            Field f = null;
            string link;
            IIndexer<NewsItem> indexer = new WordsDecorator<NewsItem>(new SearchDecorator(new NewsIndexer()));
            using (var newsdb = new NewsDb())
            {
                NewsItem newsitem =
                    newsdb.NewsItems.FirstOrDefault(item => item.CategoryName.Equals(Constants.DEFAULTMANUALCATEGORY));
                link = newsitem.Link;
                int docId = indexer.GetDocumentsIds(Constants.LINK_FIELD, link).First();
                indexer.UpdateDocField(Constants.MANUAL_CATEGORY_FIELD, categoryAux, docId);
                newsitem.CategoryName = categoryAux;
                newsdb.Entry(newsitem).State = EntityState.Modified;
                newsdb.SaveChanges();

                newsitemFinal = newsdb.NewsItems.FirstOrDefault(item => item.Link.Equals(link));
                Document d = indexer.GetReader().Document(docId);
                f = d.GetField(Constants.MANUAL_CATEGORY_FIELD);
            }
            Assert.IsTrue(newsitemFinal.CategoryName.Equals(categoryAux) && f.StringValue.Equals(categoryAux));
        }

        [TestMethod]
        public void TestRelatedNews()
        {
            using (var ctx = new NewsDb())
            {
                NewsRelationshipDecorator indexer = new NewsRelationshipDecorator(new NewsIndexer());
                //Need to put the hyperlink to the news that you need to be related
                String linkOfANews = /*PUT HERE*/string.Empty;

                //Alterar quando NewsRelated retornar o score também
                Dictionary<String,float> links = indexer.GetNewsRelated(linkOfANews);
                Assert.IsTrue(linkOfANews.Equals(string.Empty) || links.Count() != 0);
            }
        }
    }
}