using System;
using NewsBoard.Model;

namespace NewsBoard.Scraper
{
    /// <summary>
    /// This class is used for debugging purposes, in case of errors, some information produced by this 
    /// class can be usefull for bugfixing.
    /// </summary>
    public static class Logger
    {
        public static void Log(CrawlData data)
        {
            Console.WriteLine("{0} : Crawled {1} sources, {2} new feeds ({3} total)", DateTime.Now, data.TotalSources,
                data.ScrapedNow, data.TotalFeeds);
        }

        public static void Log(Exception e)
        {
            Console.WriteLine("EXCEPTION {0} : {1}", DateTime.Now, e);
        }

        public static void Log(NewsSource newsSource)
        {
            Console.WriteLine("Scraping {0} at {1}", newsSource.Name, newsSource.RssUrl);
        }

        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }

    /// <summary>
    /// Represents the result of one iteration of the Scraper module.
    /// Contains the total feeds in the database, the newly added and the total news sources.
    /// </summary>
    public struct CrawlData
    {
        public readonly int ScrapedNow;
        public readonly int TotalFeeds;
        public readonly int TotalSources;

        public CrawlData(int totalFeeds, int totalSources, int scrapedNow)
        {
            TotalFeeds = totalFeeds;
            TotalSources = totalSources;
            ScrapedNow = scrapedNow;
        }
    }
}