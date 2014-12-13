using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Search;
using NewsBoard.Indexer.Utils;
using NewsBoard.Model;
using NewsBoard.Utils;

namespace NewsBoard.Indexer
{
    /// <summary>
    ///     Funcionality to search for things
    /// </summary>
    public class SearchDecorator : IndexerDecorator<NewsItem>
    {
        private const float MINSIMILARITY = 0.3F;
        private const int PREFIXLENGHT = 2;

        public SearchDecorator(IIndexer<NewsItem> indexer)
            : base(indexer)
        {
        }

        /// <summary>
        ///     Search for a news with specific word (WITHOUT high score words filter)
        /// </summary>
        /// <param name="q">Search word</param>
        /// <returns>IEnumerable of links to the news</returns>
        public IEnumerable<String> Search(String q)
        {
            q = q.ToLower().RemoveDiacritics();
            String[] words = q.Split(' ');

            IndexReader reader = GetReader();
            var searcher = new IndexSearcher(reader);
            try
            {
                var query = new MultiPhraseQuery();
                FuzzyTermEnum fuzzyTermEnum = null;
                FuzzyTermEnum titleFuzzyTermEnum = null;
                Term auxTerm;
                foreach (String word in words)
                {
                    double lengthDiv2 = Math.Ceiling(word.Length/2.0);

                    fuzzyTermEnum = new FuzzyTermEnum(reader,
                        new Term(Constants.Constants.CONTENT_TO_SEARCH_FIELD, word),
                        MINSIMILARITY, PREFIXLENGHT);
                    do
                    {
                        auxTerm = fuzzyTermEnum.Term;

                        if (auxTerm != null && lengthDiv2 < auxTerm.Text.Length && word.Contains(auxTerm.Text))
                            break;
                        auxTerm = null;
                    } while (fuzzyTermEnum.Next());

                    if (auxTerm != null)
                        query.Add(auxTerm);
                }
                if (fuzzyTermEnum != null) fuzzyTermEnum.Dispose();
                var collector = new WithoutScoreCollector();
                searcher.Search(query, collector);
                return
                    collector.DocumentIds.Select(
                        documentId => reader.Document(documentId).GetField(Constants.Constants.LINK_FIELD).StringValue);
            }
            finally
            {
                searcher.Dispose();
            }
        }
    }
}