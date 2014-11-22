using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;
using NewsBoard.Categorizer;
using NewsBoard.Indexer;
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
        private readonly Categorize _categorizer;
        private readonly IIndexer<NewsItem> _indexer;

        private Scraper(IndexerDecorator<NewsItem> indexer)
        {
            _indexer = indexer;
            _categorizer = new Categorize(_indexer);
        }

        public static void Main(string[] args)
        {
            try
            {
                new Scraper(
                    new WordsDecorator<NewsItem>(
                        new SearchDecorator(
                            new NewsIndexer()))).Run();
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
            Console.WriteLine("Scraper started.. interval = {0}s", Constants.Constants.SleepMillis/1000);
            for (;;)
            {
                CrawlData data = ScrapSources();
                Logger.Log(data);
                Thread.Sleep(Constants.Constants.SleepMillis);
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
                    _indexer.InstantiateWriter();
                    Logger.Log(newsSource);
                    XmlReader reader = XmlReader.Create(newsSource.RssUrl);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();
                    foreach (SyndicationItem item in feed.Items)
                    {
                        String newsLink = item.Links[0].Uri.ToString();
                        if (ctx.NewsItems.Find(newsLink) != null) continue;
                        String categoryName = Constants.Constants.DEFAULTMANUALCATEGORY;
                        if (item.Categories.Count > 0)
                        {
                            categoryName = new string(item.Categories[0].Name.Where(Char.IsLetterOrDigit).ToArray());
                            if (categoryName.Contains("Fotos") || categoryName.Contains("Vídeos")) continue;
                        }
                        var newsItem = new NewsItem
                        {
                            Title = HtmlArticleExtraction.RemoveHtmlTags(item.Title.Text),
                            Description = HtmlArticleExtraction.RemoveHtmlTags(item.Summary.Text),
                            Link = newsLink,
                            PubDate = item.PublishDate.DateTime,
                            RssCategory = categoryName,
                            CategoryName = Constants.Constants.DEFAULTMANUALCATEGORY,
                            NewsSource = newsSource
                        };

                        //extract article,image,etc from news source
                        var extractor = new HtmlArticleExtraction();
                        extractor.Extract(newsLink);
                        String extractedImageUri = extractor.GetImageUri();
                        String extractedDescription = extractor.GetDescription();
                        if (extractedImageUri == null && extractedDescription == null)
                            continue;
                        if (extractedImageUri != newsSource.DefaultImageUrl)
                            newsItem.ImageLink = extractedImageUri;
                        _indexer.SetHtmlExtractor(extractor);

                        _indexer.Index(newsItem, false);
                        crawledNow++;
                        ctx.NewsItems.Add(newsItem);
                    }
                    _indexer.WriteAndDispose();
                    ctx.SaveChanges();
                }

                _categorizer.Automatic(newsCategories);
                return new CrawlData(ctx.NewsItems.Count(), newsSources.Count, crawledNow);
            }
        }
    }
}