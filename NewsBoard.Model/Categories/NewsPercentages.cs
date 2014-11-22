using System;
using System.Collections.Generic;

namespace NewsBoard.Model.Categories
{
    public class NewsPercentages
    {
        public String Link { get; set; }

        public String Category { get; set; }

        public float CategoryProbability { get; set; }

        public String RssCategory { get; set; }

        public List<String> Words { get; set; }
        public bool RssCategoryOnAdmin { get; set; }
        public bool RssCategoryOnTitle { get; set; }
        public bool RssCategoryOnArticle { get; set; }
    }
}