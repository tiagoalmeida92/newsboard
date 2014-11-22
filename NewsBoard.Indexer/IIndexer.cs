using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using NewsBoard.Utils;

namespace NewsBoard.Indexer
{
    /// <summary>
    /// Documented in Indexer.cs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIndexer<T>
    {

        void Index(T item, bool createNew);

        IEnumerable<int> GetDocumentsIds(String fieldName, String value);

        void UpdateDocField(string manualCategoryField, string manualCategoryValue, int docId);

        Analyzer GetAnalyzer();

        ISet<String> GetStopWords();

        IndexReader GetReader();

        Directory GetDirectory();

        IndexWriter GetWriter();

        void InstantiateWriter();

        void WriteAndDispose();

        void SetHtmlExtractor(HtmlArticleExtraction extractor);

        HtmlArticleExtraction GetHtmlExtractor();
    }
}