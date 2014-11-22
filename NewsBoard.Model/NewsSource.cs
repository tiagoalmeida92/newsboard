using System;
using System.ComponentModel.DataAnnotations;

namespace NewsBoard.Model
{
    public class NewsSource
    {
        [Key]
        public int Id { get; set; }

        public String Name { get; set; }

        public String RssUrl { get; set; }

        public string FaviconUrl { get; set; }

        public string SiteUrl { get; set; }
        public string DefaultImageUrl { get; set; }

    }
}