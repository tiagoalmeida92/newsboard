using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsBoard.Model;

namespace NewsBoard.Web.ViewModels
{
    public class RelatedNewsItem
    {
        public NewsItem NewsItem { get; set; }
        public int Percentage { get; set; }
    }
}