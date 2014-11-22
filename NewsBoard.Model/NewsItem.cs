using System;
using System.ComponentModel.DataAnnotations;

namespace NewsBoard.Model
{
    public class NewsItem
    {
        [Key]
        [StringLength(2000)]
        public String Link { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime PubDate { get; set; }

        public String ImageLink { get; set; }

        public String RssCategory { get; set; }

        public String CategoryName { get; set; }

        public virtual NewsSource NewsSource { get; set; }
    }
}