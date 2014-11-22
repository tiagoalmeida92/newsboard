using System;
using System.Collections.Generic;
using NewsBoard.Model;

namespace NewsBoard.Web.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<NewsItem> Results { get; set; }

        public String Query { get; set; }
        public IEnumerable<NewsSource> NewsSources { get; set; }
    }
}