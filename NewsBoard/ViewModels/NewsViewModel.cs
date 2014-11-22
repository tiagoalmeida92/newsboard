using System.Collections.Generic;
using NewsBoard.Model;

namespace NewsBoard.Web.ViewModels
{
    public class NewsViewModel
    {
        public string Category { get; set; }

        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<NewsSourceViewModel> NewsSources { get; set; }
        public bool HasFacebook { get; set; }
        public IEnumerable<FacebookPostsViewModel> FacebookPosts { get; set; }
        public string OdataEndpoint { get; set; }
    }


    public class NewsSourceViewModel
    {
        public NewsSource NewsSource { get; set; }
        public bool Ignored { get; set; }
    }
}