using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using NewsBoard.Model;
using NewsBoard.Persistence;
using NewsBoard.Utils;
using ReadSharp;

namespace NewsBoard.Scraper
{
    /// <summary>
    /// This module discovers new feeds to add to the database.
    /// Also indexes them and tries to find a matching category.
    /// </summary>
    public class Scraper
    {

        private const int SLEEP_INTERVAL = 5*60*60*1000;
        
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
            Console.WriteLine("Scraper started.. interval = {0}s", SLEEP_INTERVAL);
            for (;;)
            {
                CrawlData data = ScrapSources();
                Logger.Log(data);
                Thread.Sleep(SLEEP_INTERVAL);
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
                        var newsItem = RssToNewsItem(item);
                        newsItem.NewsSource = newsSource;
                        crawledNow++;
                        ctx.NewsItems.Add(newsItem);
                    }
                    ctx.SaveChanges();
                }
                return new CrawlData(ctx.NewsItems.Count(), newsSources.Count, crawledNow);
            }
        }

        private NewsItem RssToNewsItem(SyndicationItem item)
        {

            String newsLink = item.Links[0].Uri.ToString();
            String imageUri = GetImage(newsLink);
            NewsItem newsItem = new NewsItem
            {
                Title = Regex.Replace(item.Title.Text, @"<[^>]*>", String.Empty),
                Description = Regex.Replace(item.Summary.Text, @"<[^>]*>", String.Empty),
                Link = newsLink,
                PubDate = item.PublishDate.DateTime,
                ImageLink = imageUri
            };
            return newsItem;
        }

        static String GetImage(String articleLink)
        {
            Reader reader = new Reader();
            Article a = reader.Read(new Uri(articleLink)).Result;
            return a.FrontImage.ToString();
        }
    }
}