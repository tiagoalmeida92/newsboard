using System;
using System.Collections.Generic;
using System.Net;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using NewsBoard.Model;
using NewsBoard.Utils;

namespace NewsBoard.Indexer
{
    /// <summary>
    ///     Using Lucene .NET -
    ///     Index Files
    /// </summary>
    public class NewsIndexer : Indexer<NewsItem>
    {
        /// <summary>
        /// Method to instantiate Fields, to add Fields to a document, and add documents to a IndexWriter instance.
        /// IndexWriter instance that will write the documents to lucene files/lucene indexes.
        /// </summary>
        /// <param name="rssFeed">Type/News to be indexed and added to IndexWriter instance</param>
        /// <param name="createNew">Flag to indicate if the directory needs to be created</param>
        public override void Index(NewsItem rssFeed, bool createNew)
        {
            HtmlArticleExtraction extractor = GetHtmlExtractor();
            IndexWriter writer = GetWriter();
            var doc = new Document();
            var link = new Field(Constants.Constants.LINK_FIELD, rssFeed.Link, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var category = new Field(Constants.Constants.CATEGORY_FIELD, rssFeed.RssCategory, Field.Store.YES,
                Field.Index.NOT_ANALYZED, Field.TermVector.YES);
            var manualCategory = new Field(Constants.Constants.MANUAL_CATEGORY_FIELD, rssFeed.CategoryName,
                Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.YES);
            String titleAux = rssFeed.Title.RemovePunctuation();
            var title = new Field(Constants.Constants.TITLE_FIELD, titleAux, Field.Store.YES, Field.Index.ANALYZED,
                Field.TermVector.WITH_POSITIONS);
            String articleReader;
            try
            {
                String rawArticle = extractor.GetArticle();
                if (rawArticle != null)
                {
                    articleReader = rssFeed.Description + " " + rawArticle;
                }
                else
                {
                    articleReader = rssFeed.Description;
                }
            }
            catch (WebException)
            {
                articleReader = rssFeed.Description;
            }
            String articleAux = articleReader.RemovePunctuation();

            var article = new Field(Constants.Constants.ARTICLE_FIELD, articleAux, Field.Store.YES, Field.Index.ANALYZED,
                Field.TermVector.WITH_POSITIONS);
            var contentToSearch = new Field(Constants.Constants.CONTENT_TO_SEARCH_FIELD, titleAux + " " + articleAux,
                Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS);
            doc.Add(link);
            doc.Add(category);
            doc.Add(manualCategory);
            doc.Add(title);
            doc.Add(article);
            doc.Add(contentToSearch);
            writer.AddDocument(doc);
        }
    }
}