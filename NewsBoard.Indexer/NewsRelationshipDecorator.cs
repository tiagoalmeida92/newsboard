using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Search.Similar;
using NewsBoard.Indexer.Utils;
using NewsBoard.Model;
using Version = Lucene.Net.Util.Version;

namespace NewsBoard.Indexer
{
    public class NewsRelationshipDecorator : IndexerDecorator<NewsItem>
    {
        //Constant to filter results from the search (Query MoreLikeThis)
        private const double MINSIMILARITY = 0.25;

        public NewsRelationshipDecorator(IIndexer<NewsItem> indexer)
            : base(indexer)
        {
        }

        /// <summary>
        /// Method to get the related news of a news
        /// </summary>
        /// <param name="link">News hyperlink to get related news</param>
        /// <returns>All news related Links</returns>
        public Dictionary<String,float> GetNewsRelated(String link)
        {
            IndexReader reader = GetReader();
            IEnumerable<int> docsId = GetDocumentsIds(Constants.Constants.LINK_FIELD, link);
            IList<int> ints = docsId as IList<int> ?? docsId.ToList();
            int docId = ints.FirstOrDefault();
            if (!ints.Any()) return null;
            var mlt = new MoreLikeThis(reader);
            mlt.Analyzer = new OntoPtAnalyzer(Version.LUCENE_30, GetStopWords());
            mlt.MinTermFreq = 2;
            mlt.MinDocFreq = 1;
            mlt.SetFieldNames(new[] {Constants.Constants.TITLE_FIELD, Constants.Constants.CONTENT_TO_SEARCH_FIELD});
            Query query = mlt.Like(docId);
            var searcher = new IndexSearcher(reader);
            TopDocs hitsFound = searcher.Search(query, 4);
            ScoreDoc[] scoreDocs = hitsFound.ScoreDocs;
            return scoreDocs.Where(item => item.Doc != docId && item.Score > MINSIMILARITY).ToDictionary(item=> reader.Document(item.Doc).GetField(Constants.Constants.LINK_FIELD).StringValue,item =>item.Score );
        }
    }            

}