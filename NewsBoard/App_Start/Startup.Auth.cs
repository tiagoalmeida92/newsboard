using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Twitter;
using Owin;

namespace NewsBoard.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TwitterConsumerKey")))
            {
                var twitterOptions = new TwitterAuthenticationOptions
                {
                    ConsumerKey = ConfigurationManager.AppSettings.Get("TwitterConsumerKey"),
                    ConsumerSecret = ConfigurationManager.AppSettings.Get("TwitterConsumerSecret"),
                    Provider = new TwitterAuthenticationProvider
                    {
                        OnAuthenticated = context =>
                        {
                            context.Identity.AddClaim(new Claim("urn:twitter:access_token", context.AccessToken,
                                XmlSchemaString, "Twitter"));
                            return Task.FromResult(0);
                        }
                    }
                };
                app.UseTwitterAuthentication(twitterOptions);
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("FacebookAppId")))
            {
                ////https://www.facebook.com/dialog/oauth?client_id=686678408089891&redirect_uri=http://localhost:7720&auth_type=rerequest&scope=user_likes
                //var facebookOptions = new FacebookAuthenticationOptions
                //{
                //    AppId = ConfigurationManager.AppSettings.Get("FacebookAppId"),
                //    AppSecret = ConfigurationManager.AppSettings.Get("FacebookAppSecret"),
                //    Provider = new FacebookAuthenticationProvider
                //    {
                //        OnAuthenticated = context =>
                //        {
                //            string accessToken = GetFacebookExtendedAccessToken(context.AccessToken);
                //            context.Identity.AddClaim(new Claim(Constants.Constants.FACEBOOK_ACCESS_TOKEN_CLAIM_TYPE,
                //                accessToken, XmlSchemaString, "Facebook"));
                //            return Task.FromResult(0);
                //        }
                //    }
                //};
                //facebookOptions.Scope.Add("email");
                //facebookOptions.Scope.Add("user_likes");
                //app.UseFacebookAuthentication(facebookOptions);
            }

//            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("GooglePlusClientId")))
//            {
//                app.UseGooglePlusAuthentication(ConfigurationManager.AppSettings.Get("GooglePlusClientId"), ConfigurationManager.AppSettings.Get("GooglePlusClientSecret"));
//            }
        }

        /// <summary>
        ///     Extend 60 days token
        /// </summary>
        /// <param name="shortlivedtoken"></param>
     
    }
}