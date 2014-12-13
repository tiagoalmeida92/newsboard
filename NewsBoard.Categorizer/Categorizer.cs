using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using NewsBoard.Indexer;
using NewsBoard.Model;
using NewsBoard.Model.Categories;
using NewsBoard.Persistence;

namespace NewsBoard.Categorizer
{
    /// <summary>
    ///     Class that is responsible by manual categorization (by admin or the programmer)
    ///     And auto categorization ('Geral' category to specific category)
    /// </summary>
    public class Categorize
    {
        private readonly WordsDecorator<NewsItem> _indexerManual;

        public Categorize(IIndexer<NewsItem> indexer)
        {
            _indexerManual = (WordsDecorator<NewsItem>) indexer;
        }

        /// <summary>
        ///     Set category automatic (all news that have 'Undifferentiated' as manual category)
        /// </summary>
        /// <param name="manualCategories">List of categories defined by developers</param>
        public int Automatic(IList<NewsCategory> manualCategories)
        {
            //First criteria
            //For all undifferentiated news
            IList<IGrouping<string, NewsPercentages>> probabilities = CalculateCategoryProbilities(manualCategories);

            //Auto associate news with categories
            if (probabilities.Count != 0)
                return AssociateNewsWithCategories(probabilities, manualCategories);
            return 0;
        }

        /// <summary>
        ///     Automatically associate the news, with the categories
        /// </summary>
        /// <param name="probabilities">
        ///     List of news with the probability of the category and the category associated with this
        ///     probability
        /// </param>
        /// <param name="manualCategories">List of categories defined by developers</param>
        private int AssociateNewsWithCategories(IList<IGrouping<string, NewsPercentages>> probabilities, IList<NewsCategory> manualCategories)
        {
            int countAssociated=0;
            using (var newsdb = new NewsDb())
            {
                foreach (var percentage in probabilities)
                {
                    NewsPercentages final;
                    if (
                        percentage.Count(
                            item =>
                                item.CategoryProbability >=
                                Constants.Constants.PERCENTAGE_LIMIT_TO_CONDITION_TO_ASSOCIATE) > 1)
                    {
                        float max = percentage.Max(item => item.CategoryProbability);
                        if (percentage.Count(item => item.CategoryProbability.Equals(max)) > 1)
                            continue;
                        final = percentage.First(item => item.CategoryProbability.Equals(max));
                    }
                    else
                        final =
                            percentage.First(
                                item =>
                                    item.CategoryProbability >=
                                    Constants.Constants.PERCENTAGE_LIMIT_TO_CONDITION_TO_ASSOCIATE);

                    if (final.CategoryProbability >= Constants.Constants.PERCENTAGE_WHEN_ASSOCIATE_CATEGORY_TO_NEWS
                        || CountRssCategoryOnWords(final) > 1 ||
                        final.RssCategory.Equals(final.Category, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ++countAssociated;
                        _indexerManual.InstantiateWriter();
                        string link = final.Link;
                        string category = final.Category;
                        NewsItem newsItem = newsdb.NewsItems.Find(link);
                        int docId = _indexerManual.GetDocumentsIds(Constants.Constants.LINK_FIELD, link).Single();
                        _indexerManual.UpdateDocField(Constants.Constants.MANUAL_CATEGORY_FIELD, category, docId);
                        newsItem.CategoryName = category;
                        newsdb.Entry(newsItem).State = EntityState.Modified;
                        _indexerManual.WriteAndDispose();
                        newsdb.SaveChanges();
                    }
                }
            }
            return countAssociated;
        }


        private int CountRssCategoryOnWords(NewsPercentages np)
        {
            int count = 0;
            if (np.RssCategoryOnAdmin) ++count;
            if (np.RssCategoryOnArticle) ++count;
            if (np.RssCategoryOnTitle) ++count;
            return count;
        }

        /// <summary>
        ///     Calculate categories probabilities for all undifferentiated news
        /// </summary>
        /// <param name="manualCategories">List of news and categories probabilities</param>
        private IList<IGrouping<string, NewsPercentages>> CalculateCategoryProbilities(
            IList<NewsCategory> manualCategories)
        {
            IList<NewsPercentages> probabilities = new List<NewsPercentages>();
            Dictionary<String, NewsPercentages> articleTopWordDictPerNews =
                _indexerManual.GetTopWordsPerNewsLink(Constants.Constants.ARTICLE_FIELD,
                    Constants.Constants.MANUAL_CATEGORY_FIELD, Constants.Constants.DEFAULTMANUALCATEGORY);

            Dictionary<String, NewsPercentages> titleTopWordDictPerNews =
                _indexerManual.GetTopWordsPerNewsLink(Constants.Constants.TITLE_FIELD,
                    Constants.Constants.MANUAL_CATEGORY_FIELD, Constants.Constants.DEFAULTMANUALCATEGORY);

            IEnumerable<IGrouping<string, KeyValuePair<string, NewsPercentages>>> dict4 = articleTopWordDictPerNews
                .Union(titleTopWordDictPerNews)
                .GroupBy(kvp => kvp.Key);

            var final = new Dictionary<string, NewsPercentages>();

            foreach (var pair in dict4)
            {
                string link = pair.Key;
                NewsPercentages np = pair.First().Value;
                np.Words = pair.SelectMany(item => item.Value.Words).ToList();
                final.Add(link, np);
            }

            int titleNewsCount, articleNewsCount;
            foreach (NewsCategory manualCategory in manualCategories)
            {
                //get category words information
                List<String> adminWords = _indexerManual.GetWords(Constants.Constants.ADMINSUBJECTS_FIELD,
                    Constants.Constants.MANUAL_CATEGORY_FIELD, manualCategory.Name);
                List<String> titleWords = _indexerManual.GetWords(Constants.Constants.TITLE_FIELD,
                    Constants.Constants.MANUAL_CATEGORY_FIELD, manualCategory.Name);
                List<String> articleWords = _indexerManual.GetWords(Constants.Constants.ARTICLE_FIELD,
                    Constants.Constants.MANUAL_CATEGORY_FIELD, manualCategory.Name);
                int adminNr, titleNr, articleNr;
                if (adminWords.Count == 0 && titleWords.Count == 0 && articleWords.Count == 0)
                    continue;

                foreach (var pair in final)
                {
                    adminNr = 0;
                    titleNr = 0;
                    articleNr = 0;
                    bool rssCatOnAdmin = false;
                    bool rssCatOnTitle = false;
                    bool rssCatOnArticle = false;
                    string rssCat = pair.Value.RssCategory;
                    pair.Value.Words.ForEach(word =>
                    {
                        adminNr += adminWords.Count(adminWord =>
                        {
                            if (adminWord.Equals(rssCat, StringComparison.InvariantCultureIgnoreCase))
                                rssCatOnAdmin = true;
                            if (adminWord.Equals(word, StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (char.IsUpper(adminWord[0]))
                                    ++adminNr;
                                if (char.IsUpper(word[0]))
                                    ++adminNr;
                                return true;
                            }
                            return false;
                        });
                        titleNr += titleWords.Count(titleWord =>
                        {
                            if (titleWord.Equals(rssCat, StringComparison.InvariantCultureIgnoreCase))
                                rssCatOnTitle = true;
                            if (titleWord.Equals(word, StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (char.IsUpper(titleWord[0]))
                                    ++titleNr;
                                if (char.IsUpper(word[0]))
                                    ++titleNr;
                                return true;
                            }
                            return false;
                        });
                        articleNr += articleWords.Count(articleWord =>
                        {
                            if (articleWord.Equals(rssCat, StringComparison.InvariantCultureIgnoreCase))
                                rssCatOnArticle = true;
                            if (articleWord.Equals(word, StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (char.IsUpper(articleWord[0]))
                                    ++articleNr;
                                if (char.IsUpper(word[0]))
                                    ++articleNr;
                                return true;
                            }
                            return false;
                        });
                    });
                    float adminPercentage = (float) adminNr/adminWords.Count;
                    float articlePercentage = (float) articleNr/articleWords.Count;
                    float titlePercentage = (float) titleNr/titleWords.Count;
                    float categoryprobability = CalculateCategoryProbability(adminPercentage, titlePercentage,
                        articlePercentage);
                    probabilities.Add(new NewsPercentages
                    {
                        Category = manualCategory.Name,
                        CategoryProbability = categoryprobability,
                        Link = pair.Value.Link,
                        RssCategory = pair.Value.RssCategory,
                        RssCategoryOnAdmin = rssCatOnAdmin,
                        RssCategoryOnArticle = rssCatOnArticle,
                        RssCategoryOnTitle = rssCatOnTitle
                    });
                }
                List<NewsPercentages> newList =
                    probabilities.OrderByDescending(item => item.CategoryProbability)
                        .Where(
                            news =>
                                news.Category.Equals(manualCategory.Name) &&
                                news.CategoryProbability >=
                                Constants.Constants.PERCENTAGE_LIMIT_TO_CONDITION_TO_ASSOCIATE)
                        .ToList();
            }

            return
                probabilities.GroupBy(comp => comp.Link)
                    .Where(
                        news =>
                            news.Count(
                                item =>
                                    item.CategoryProbability >=
                                    Constants.Constants.PERCENTAGE_LIMIT_TO_CONDITION_TO_ASSOCIATE) > 0)
                    .ToList();
        }

        /// <summary>
        ///     Calculate category probability for a news percentages
        /// </summary>
        /// <param name="adminPercentage">Percentage of words (of a category) put by admin in News</param>
        /// <param name="titlePercentage">Percentage of words (of a category) present in the title of a news</param>
        /// <param name="articlePercetage">Percentage of words (of a category) present in the article of a news</param>
        /// <returns></returns>
        private float CalculateCategoryProbability(float adminPercentage, float titlePercentage, float articlePercetage)
        {
            return (adminPercentage*Constants.Constants.PERCENTAGE_CATEGORY_ADMINWORDS)
                   + (titlePercentage*Constants.Constants.PERCENTAGE_CATEGORY_TITLEWORDS)
                   + (articlePercetage*Constants.Constants.PERCENTAGE_CATEGORY_ARTICLEWORDS);
        }

        /// <summary>
        ///    @Deprecated - Set category manual (individual news)
        /// </summary>
        /// <param name="newsItem">A news object</param>
        /// <param name="manualCategories">List of categories defined by developers</param>
        public void Manual(ref NewsItem newsItem, IList<NewsCategory> manualCategories)
        {
            NewsItem aux = newsItem;
            if (
                manualCategories.Any(
                    tuple => tuple.Name.Equals(aux.RssCategory, StringComparison.InvariantCultureIgnoreCase)))
                newsItem.CategoryName = newsItem.RssCategory;
            else
            {
                string urlEncode = HttpUtility.UrlEncode(aux.RssCategory, Encoding.GetEncoding(28597));
                string categoryNAccents = null;
                if (urlEncode != null)
                {
                    categoryNAccents = urlEncode
                        .Replace("+", " ");
                }
                NewsCategory tuple =
                    manualCategories.FirstOrDefault(
                        category =>
                            category.SubCategories.Any(
                                rssCateg =>
                                    aux.RssCategory.Contains(rssCateg.Name) ||
                                    (categoryNAccents != null && categoryNAccents.Contains(rssCateg.Name))));
                newsItem.CategoryName = tuple != null ? tuple.Name : Constants.Constants.DEFAULTMANUALCATEGORY;
            }
        }
    }
}