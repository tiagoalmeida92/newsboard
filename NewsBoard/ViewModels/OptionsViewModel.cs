using System.Collections.Generic;
using NewsBoard.Web.Controllers;

namespace NewsBoard.Web.ViewModels
{
    public class OptionsViewModel
    {
        public IEnumerable<int> IgnoredSources { get; set; }
        public NewsController.Order Order { get; set; }
    }
}