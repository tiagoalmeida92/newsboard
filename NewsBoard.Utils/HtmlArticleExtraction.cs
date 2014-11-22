using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NReadability;
using ReadSharp;

namespace NewsBoard.Utils
{
    /// <summary>
    ///     Using HtmlAgilityPack and NReadability
    ///     To fullarticle/title/uri of the article image extraction
    /// </summary>
    public class HtmlArticleExtraction
    {
        //private readonly HtmlDocument _htmlDoc;
        private readonly NReadabilityWebTranscoder _transcoder;
        private bool _serverDown = false;
        private Article _article;

        /// <summary>
        ///     To Extract text from Html Page (news page)
        /// </summary>
        public HtmlArticleExtraction()
        {
            _transcoder = new NReadabilityWebTranscoder();
        }

        /// <summary>
        /// Utility method to extract html tags from a string
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>A string without html tags</returns>
        public static string RemoveHtmlTags(string str)
        {
            return Regex.Replace(str, @"<[^>]*>", String.Empty);
        }

        /// <summary>
        /// Method to extract aditional text from a NewsItem in the NewsSource website
        /// Used for indexing purposes
        /// </summary>
        public void Extract(String uri)
        {
            try
            {
                Reader reader = new Reader();
                ReadOptions options = new ReadOptions();
                options.PreferHTMLEncoding = false;
                var tokenSource = new CancellationTokenSource();
                Task<Article> task = reader.Read(new Uri(uri), options, tokenSource.Token);
                _article = task.Result;
                tokenSource.Cancel();
                if (_article.Title.Equals("Advertisement"))
                {
                    _article = reader.Read(new Uri(uri), options).Result;
                }
            }
            catch (ReadException)
            {
                _serverDown = true;
            }
        }
        
        /// <summary>
        /// Get the news article after the extraction is complete
        /// </summary>
        /// <returns>News title</returns>
        public String GetArticle()
        {
            return _serverDown?null:RemoveHtmlTags(_article.Content);
        }

        /// <summary>
        /// Get the the news title after the extraction is complete
        /// </summary>
        /// <returns>News title</returns>
        public String GetTitle()
        {
            return _serverDown?null:_article.Title;
        }

        /// <summary>
        /// Get the the news image after the extraction is complete
        /// </summary>
        /// <returns>Image Uri</returns>
        public String GetImageUri()
        {
            if (_article.FrontImage != null && !_serverDown)
                return _article.FrontImage.ToString();
            return null;
        }

        /// <summary>
        /// Get the the news image after the extraction is complete
        /// </summary>
        /// <returns>Image Uri</returns>
        public String GetDescription()
        {
            return _serverDown?null:_article.Description;
        }
    }
}