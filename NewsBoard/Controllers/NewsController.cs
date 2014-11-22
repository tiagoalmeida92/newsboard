using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NewsBoard.Persistence;
using NewsBoard.Utils;
using NewsBoard.Web.Models;
using NewsBoard.Web.ViewModels;

namespace NewsBoard.Web.Controllers
{
    public class NewsController : Controller
    {
        public enum Order
        {
            NewestFirst,
            AToZ,
            ZToA
        }

        private NewsDb _db = new NewsDb();

        private UserManager<ApplicationUser> _manager =
            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        // GET: /News
        public ActionResult Index(string category, Order order = Order.NewestFirst)
        {
            var vm = new NewsViewModel
            {
                Categories = _db.NewsCategories.Select(c => c.Name),
                Category = category,
                NewsSources = _db.NewsSources.Select(ns => new NewsSourceViewModel {NewsSource = ns}).ToList(),
                HasFacebook = false
            };
            ODataQueryBuilder oDataQueryBuilder = ODataQueryBuilder.From("/odata/NewsItems")
                .Expand("NewsSource");
            switch (order)
            {
                case Order.NewestFirst:
                    oDataQueryBuilder.OrderBy("PubDate", ODataQueryBuilder.Order.desc);
                    break;
                case Order.AToZ:
                    oDataQueryBuilder.OrderBy("Title", ODataQueryBuilder.Order.asc);
                    break;
                case Order.ZToA:
                    oDataQueryBuilder.OrderBy("Title", ODataQueryBuilder.Order.desc);
                    break;
            }
            if (!category.IsNullOrWhiteSpace())
            {
                oDataQueryBuilder.FilterAndEq("CategoryName", category);
            }
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = _manager.FindById(User.Identity.GetUserId());
                foreach (int ns in user.IgnoredNewsSourcesIds)
                {
                    NewsSourceViewModel sourceView = vm.NewsSources.First(n => ns == n.NewsSource.Id);
                    sourceView.Ignored = true;
                    oDataQueryBuilder.FilterAnd("NewsSource/Id ne " + sourceView.NewsSource.Id);
                }
            }
            vm.OdataEndpoint = oDataQueryBuilder.Build();
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = _manager.FindById(User.Identity.GetUserId());
                foreach (int ns in user.IgnoredNewsSourcesIds)
                {
                    NewsSourceViewModel sourceView = vm.NewsSources.First(n => ns == n.NewsSource.Id);
                    sourceView.Ignored = true;
                }
                //if (category.IsNullOrWhiteSpace())
                //{
                //    RedirectResult redirect = GetFacebookLikes(vm);
                //    if (redirect != null)
                //        return redirect;
                //}
            }
            return View(vm);
        }

        //private RedirectResult GetFacebookLikes(NewsViewModel vm)
        //{
        //    //constant to get all user_likes that contain word news
        //    const string NEWS = "news";
        //    //constant to filter only the facebook links (not the photos)
        //    const string LINK = "link";

        //    //ACCESS TOKEN TO GET USER LIKES(facebook)/FOLLOWING(twitter) AND SEE NEWS POSTS
        //    var claimsIdentity = User.Identity as ClaimsIdentity;
        //    Claim claim = claimsIdentity.FindFirst("urn:facebook:access_token");
        //    string accessToken = claim != null ? claim.Value : null;
        //    if (accessToken != null)
        //    {
        //        vm.HasFacebook = true;
        //        try
        //        {
        //            var fb = new FacebookClient(accessToken);
        //            var likes = fb.Get<FacebookLikesCrate>("/me/likes?limit=1000");

        //            if (likes != null && likes.Data.Count != 0)
        //            {
        //                List<FacebookLikesModel> data = likes.Data;
        //                //category=Media/News/Publishing
        //                List<string> news =
        //                    data.Where(item => item.Category.Contains(NEWS)).Select(item2 => item2.ID).ToList();
        //                IEnumerable<FacebookPostsViewModel> captions = new List<FacebookPostsViewModel>();
        //                foreach (string str in news)
        //                {
        //                    var posts = fb.Get<FacebookPostsCrate>("/" + str + "/posts");
        //                    if (posts != null)
        //                    {
        //                        List<FacebookPostsViewModel> postsData = posts.Data;
        //                        string url;

        //                        FacebookPostsViewModel facebookPostsViewModel =
        //                            postsData.FirstOrDefault(
        //                                item =>
        //                                    item.Caption != null &&
        //                                    Uri.IsWellFormedUriString(item.Caption, UriKind.RelativeOrAbsolute));
        //                        if (facebookPostsViewModel == null)
        //                            continue;
        //                        url = facebookPostsViewModel.Caption;
        //                        captions = captions.Concat(postsData.Where(item => item.Type.Equals(LINK)));
        //                        //if (vm.NewsSources.Any(source =>
        //                        //{
        //                        //    string sourceUrl = source.NewsSource.SiteUrl;
        //                        //    var uri = new Uri(sourceUrl);
        //                        //    string uriWithoutScheme = uri.Host;
        //                        //    return //!source.Ignored &&
        //                        //           uriWithoutScheme.Equals(url, StringComparison.InvariantCultureIgnoreCase);
        //                        //}))
        //                        //{
        //                        //    captions = captions.Concat(postsData.Where(item => item.Type.Equals(LINK)));
        //                        //}
        //                    }
        //                }
        //                vm.FacebookPosts = captions;
        //            }
        //        }
        //        catch (FacebookOAuthException)
        //        {
        //            return Redirect("https://www.facebook.com/dialog/oauth?" +
        //                            "client_id=" + ConfigurationManager.AppSettings.Get("FacebookAppId") +
        //                            "&redirect_uri=" + Url.Action("Login", "Account", null,Request.Url.Scheme) +
        //                            "&scope=user_likes");
        //        }
        //    }
        //    return null;
        //}


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}