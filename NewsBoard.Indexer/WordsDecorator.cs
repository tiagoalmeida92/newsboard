using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using NewsBoard.Indexer.Model;
using NewsBoard.Indexer.Utils;
using NewsBoard.Model.Categories;

namespace NewsBoard.Indexer
{
    /// <summary>
    ///     Concret decorator to add category funcionalities
    /// </summary>
    public class WordsDecorator<T> : IndexerDecorator<T>
    {
        public WordsDecorator(IIndexer<T> indexer)
            : base(indexer)
        {
        }

        /// <summary>
        ///     Method to get all terms/words from a specific Field Name(wordsFieldName)
        ///     of a specific document (fieldName,fieldValue)
        /// </summary>
        /// <param name="wordsFieldName">Field name in which we get the words</param>
        /// <param name="fieldName">Field name (all cases is Link field Name)</param>
        /// <param name="fieldValue">Field value (all cases is link value)</param>
        /// <returns></returns>
        public List<String> GetWords(String wordsFieldName, String fieldName, String fieldValue)
        {
            IndexReader reader = GetReader();
            IEnumerable<int> docsId = GetDocumentsIds(fieldName, fieldValue);
            var disctinctedDic = new Dictionary<String, FrequencyAndDocsCount>();
            foreach (int docId in docsId)
            {
                ITermFreqVector vectorTf = reader.GetTermFreqVector(docId, wordsFieldName);
                if (vectorTf != null)
                {
                    String[] termList = vectorTf.GetTerms();
                    int[] freqList = vectorTf.GetTermFrequencies();
                    WordsDistinguisher.DistinguishWords(termList, freqList, ref disctinctedDic);
                }
            }
            return disctinctedDic.Select(item => item.Key).ToList();
        }

        /// <summary>
        ///     Get top words of manual category. Most frequent words is defined as the terms whose frequencies
        ///     are greater or equal to the topTermLimit. Return: List of category top words
        /// </summary>
        /// <param name="wordsFieldName">Field Name to get the top words</param>
        /// <param name="fieldCategoryName">Field name of category value (manual category or rss category type)</param>
        /// <param name="category">Category name</param>
        /// <param name="newsCount">Number of news viewed</param>
        /// <returns>List of top words of a specific category</returns>
        public List<String> GetTopWordsFilter(String wordsFieldName, String fieldCategoryName, String category,
            out int newsCount, float topTermLimit = 0.8F)
        {
            newsCount = 0;
            IndexReader reader = GetReader();
            //1-Get only the higher frequencies from all documents
            var frequencyDic = new Dictionary<String, FrequencyAndDocsCount>();
            foreach (int documentId in GetDocumentsIds(fieldCategoryName, category))
            {
                ++newsCount;
                ITermFreqVector vectorTf = reader.GetTermFreqVector(documentId, wordsFieldName);
                String[] termList = vectorTf.GetTerms();
                int[] freqList = vectorTf.GetTermFrequencies();
                WordsDistinguisher.GetDocsTopWords(termList, freqList, ref frequencyDic);
            }

            //2-Get only the  words with higher frequency from the specified category
            IOrderedEnumerable<KeyValuePair<string, FrequencyAndDocsCount>> ordered = frequencyDic.OrderByDescending(
                pair => pair.Value.GetDocFreq())
                .OrderByDescending(pair => pair.Value.GetDocCount());

            var higherWordsList = new List<string>();
            WordsDistinguisher.GetTopWords(ordered, higherWordsList, topTermLimit);

            return higherWordsList;
        }


        /// <summary>
        ///    Get Top Words per news link.
        ///    To get article, title top words of all news that as a specific category (not rss category).
        ///    This method is to automatic categorization.  
        /// </summary>
        /// <param name="wordsFieldName">Field name which withdraws the 'top words'.</param>
        /// <param name="manualCategoryField">Field name of category (all cases Manual Category name)</param>
        /// <param name="defaultmanualcategory">Field value of category</param>
        /// <returns>Dictionary of news hyperlinks and type NewsPercentages</returns>
        public Dictionary<String, NewsPercentages> GetTopWordsPerNewsLink(String wordsFieldName,
            String manualCategoryField,
            String defaultmanualcategory)
        {
            var percentages = new Dictionary<String, NewsPercentages>();
            IndexReader reader = GetReader();
            Action<int, List<String>> addFunc = (docId, list) =>
            {
                Document d = reader.Document(docId);
                Field f = d.GetField(Constants.Constants.LINK_FIELD);
                Field f2 = d.GetField(Constants.Constants.CATEGORY_FIELD);
                String link = f.StringValue;
                String rssCat = f2.StringValue;
                if (link != null && rssCat != null)
                {
                    percentages.Add(link, new NewsPercentages { Words = list, Link = link, RssCategory = rssCat });
                }
            };
            GetTopWordsPerNews(wordsFieldName, manualCategoryField, defaultmanualcategory, addFunc);

            return percentages;
        }

        /// <summary>
        ///     Method to be called by GetTopWordsPerNewsLink method
        ///     Created in case of change the Action 'addFunc'.
        /// </summary>
        /// <param name="wordsFieldName">Field name which withdraws the 'top words'.</param>
        /// <param name="manualCategoryField">Field name of category (all cases Manual Category name)</param>
        /// <param name="defaultmanualcategory">Field value of category</param>
        /// <param name="addFunc"></param>
        private void GetTopWordsPerNews(String wordsFieldName, String manualCategoryField, String defaultmanualcategory,
            Action<int, List<string>> addFunc)
        {
            const float TOPTERMLIMIT = 0.7F;
            IndexReader reader = GetReader();
            IList<int> docsIds = GetDocumentsIds(manualCategoryField, defaultmanualcategory).ToList();
            foreach (int docId in docsIds)
            {
                var auxDict = new Dictionary<string, FrequencyAndDocsCount>();
                ITermFreqVector freqVector = reader.GetTermFreqVector(docId, wordsFieldName);
                if (freqVector == null)
                    Console.WriteLine("Error");
                String[] terms = freqVector.GetTerms();
                int[] freqs = freqVector.GetTermFrequencies();
                WordsDistinguisher.GetDocsTopWords(terms, freqs, ref auxDict, TOPTERMLIMIT);
                addFunc(docId, auxDict.Select(pair => pair.Key).ToList());
            }
        }
    }
}