using System.Collections.Generic;
using System.Data.Entity.Migrations;
using NewsBoard.Model;

namespace NewsBoard.Persistence.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<NewsDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NewsDb context)
        {
            context.NewsCategories.AddOrUpdate(category => category.Name, new[]
            {
                new NewsCategory
                {
                    Name = "Desporto",
                    SubCategories =
                        new List<CategoryData>
                        {
                            new CategoryData("Outras modalidades"),
                            new CategoryData("Futebol"),
                            new CategoryData("Benfica"),
                            new CategoryData("Sporting"),
                            new CategoryData("Porto")
                        }
                },
                new NewsCategory
                {
                    Name = "Política",
                    SubCategories =
                        new List<CategoryData>
                        {
                            new CategoryData("PSD"),
                            new CategoryData("PS"),
                            new CategoryData("Outros")
                        }
                },
                new NewsCategory
                {
                    Name = "Tecnologia",
                    SubCategories =
                        new List<CategoryData>
                        {
                            new CategoryData("Apple"),
                            new CategoryData("Android"),
                            new CategoryData("Infomática")
                        }
                },
                new NewsCategory
                {
                    Name = "Economia",
                    SubCategories =
                        new List<CategoryData>
                        {
                            new CategoryData("Euro"),
                            new CategoryData("Banca"),
                            new CategoryData("Internacional")
                        }
                },
                new NewsCategory
                {
                    Name = "Internacional",
                    SubCategories =
                        new List<CategoryData>
                        {
                            new CategoryData("Mundo"),
                            new CategoryData("Globo"),
                            new CategoryData("Europa"),
                            new CategoryData("EUA e Américas")
                        }
                },
                new NewsCategory
                {
                    Name = "Nacional",
                    SubCategories =
                        new List<CategoryData>
                        {
                            new CategoryData("Portugal"),
                            new CategoryData("Madeira"),
                            new CategoryData("Açores")
                        }
                }
            });
            context.NewsSources.AddOrUpdate(ns => ns.Name, new[]
            {
                new NewsSource
                {
                    Name = "Publico",
                    RssUrl = "http://feeds.feedburner.com/PublicoRSS",
                    FaviconUrl = "http://s2.googleusercontent.com/s2/favicons?domain=http://www.publico.pt/",
                    SiteUrl = "http://www.publico.pt",
                    DefaultImageUrl = "http://static.publico.pt/files/homepage/img/logo_share.png"
                },
                new NewsSource
                {
                    Name = "TSF",
                    RssUrl = "http://feeds.tsf.pt/TSF-Ultimas",
                    FaviconUrl = "http://s2.googleusercontent.com/s2/favicons?domain=http://www.tsf.pt",
                    SiteUrl = "http://www.tsf.pt",
                    DefaultImageUrl = "http://www.tsf.pt/common/images/TSF_DEFAULT_IMG.jpg"
                },
                new NewsSource
                {
                    Name = "DN",
                    RssUrl = "http://feeds.dn.pt/DN-Ultimas",
                    SiteUrl = "http://www.dn.pt",
                    FaviconUrl = "http://s2.googleusercontent.com/s2/favicons?domain=http://www.dn.pt",
                    DefaultImageUrl = "http://www.dn.pt/common/images/DN_DEFAULT_IMG.jpg"
                },
                new NewsSource
                {
                    Name = "Abola",
                    RssUrl = "http://rss.feedsportal.com/c/32502/f/480420/index.rss",
                    FaviconUrl = "http://s2.googleusercontent.com/s2/favicons?domain=http://www.abola.pt",
                    SiteUrl = "http://www.abola.pt",
                    DefaultImageUrl = "http://www.dn.pt/common/images/DN_DEFAULT_IMG.jpg"
                },
                new NewsSource
                {
                    RssUrl = "http://feeds.record.xl.pt/?idcanal=",
                    Name = "Record",
                    SiteUrl = "http://www.record.xl.pt/",
                    FaviconUrl = "http://s2.googleusercontent.com/s2/favicons?domain=http://www.record.xl.pt/",
                    DefaultImageUrl = "http://www.dn.pt/common/images/DN_DEFAULT_IMG.jpg"
                },
                new NewsSource
                {
                    RssUrl = "http://pplware.sapo.pt/feed/",
                    Name = "Pplware",
                    SiteUrl = "http://pplware.sapo.pt/",
                    FaviconUrl = "http://s2.googleusercontent.com/s2/favicons?domain=http://pplware.sapo.pt/",
                    DefaultImageUrl = ""
                },
                new NewsSource
                {
                    Name = "Exame Informática",
                    SiteUrl = "http://exameinformatica.sapo.pt/",
                    RssUrl = "http://feeds.feedburner.com/ExameInformatica-geral",
                    FaviconUrl = "http://s2.googleusercontent.com/s2/favicons?domain=http://exameinformatica.sapo.pt/",
                    DefaultImageUrl = ""
                }
            });
        }
    }
}