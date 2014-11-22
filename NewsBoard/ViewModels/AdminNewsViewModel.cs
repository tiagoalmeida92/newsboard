using System.Collections.Generic;
using NewsBoard.Model;

namespace NewsBoard.Web.ViewModels
{
    public class AdminNewsViewModel
    {
        public IEnumerable<NewsItem> News { get; set; }

        public IEnumerable<string> NewsCategories { get; set; }
        public string Category { get; set; }
    }
}