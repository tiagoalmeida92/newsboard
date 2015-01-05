using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;
using NewsBoard.Model;
using NewsBoard.Persistence;
using NewsBoard.Utils;

namespace NewsBoard.Scraper
{
    /// <summary>
    /// This module discovers new feeds to add to the database.
    /// Also indexes them and tries to find a matching category.
    /// </summary>
    public class Scraper
    {
        
        public static void Main(string[] args)
        {
            try
            {
                new Scraper().Run();
            }
            catch (Exception e)
            {
                Logger.Log(e);
                throw;
            }
        }

        /// <summary>
        /// Aplication loop
        /// </summary>
        public void Run()
        {
            Console.WriteLine("Scraper started.. interval = {0}s", 5 * 60 * 60);
            for (;;)
            {
                CrawlData data = ScrapSources();
                Logger.Log(data);
                Thread.Sleep(5*60*60);
            }
        }

        /// <summary>
        /// Main scraper function
        /// Finds new feeds, adds them to database, indexes them and finally categorize each one.
        /// </summary>
        /// <returns></returns>
        private CrawlData ScrapSources()
        {
            int crawledNow = 0;
            using (var ctx = new NewsDb())
            {
                IList<NewsCategory> newsCategories = ctx.NewsCategories.ToList();
                IList<NewsSource> newsSources = ctx.NewsSources.ToList();

                foreach (NewsSource newsSource in newsSources)
                {
                    Logger.Log(newsSource);
                    XmlReader reader = XmlReader.Create(newsSource.RssUrl);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();
                    foreach (SyndicationItem item in feed.Items)
                    {
                        String newsLink = item.Links[0].Uri.ToString();
                        if (ctx.NewsItems.Find(newsLink) != null) continue;
                       
                        var newsItem = new NewsItem
                        {
                            Title = item.Title.Text,
                            Description = item.Summary.Text,
                            Link = newsLink,
                            PubDate = item.PublishDate.DateTime,
                            NewsSource = newsSource
                        };
                        crawledNow++;
                        ctx.NewsItems.Add(newsItem);
                    }
                    ctx.SaveChanges();
                }
                return new CrawlData(ctx.NewsItems.Count(), newsSources.Count, crawledNow);
            }
        }
    }
}