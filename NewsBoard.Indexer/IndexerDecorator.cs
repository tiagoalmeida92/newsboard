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
    public abstract class IndexerDecorator<T> : IIndexer<T>
    {
        private readonly IIndexer<T> _indexer; //indexer being decorated

        protected IndexerDecorator(IIndexer<T> indexer)
        {
            _indexer = indexer;
        }

        public void Index(T item, bool createNew)
        {
            _indexer.Index(item, createNew);
        }

        public IEnumerable<int> GetDocumentsIds(string fieldName, string value)
        {
            return _indexer.GetDocumentsIds(fieldName, value);
        }

        public void UpdateDocField(string field, string value, int docId)
        {
            _indexer.UpdateDocField(field, value, docId);
        }

        public Directory GetDirectory()
        {
            return _indexer.GetDirectory();
        }

        public IndexWriter GetWriter()
        {
            return _indexer.GetWriter();
        }

        public void InstantiateWriter()
        {
            _indexer.InstantiateWriter();
        }

        public void WriteAndDispose()
        {
            _indexer.WriteAndDispose();
        }

        public void SetHtmlExtractor(HtmlArticleExtraction extractor)
        {
            _indexer.SetHtmlExtractor(extractor);
        }

        public HtmlArticleExtraction GetHtmlExtractor()
        {
            return _indexer.GetHtmlExtractor();
        }

        public IndexReader GetReader()
        {
            return _indexer.GetReader();
        }

        public ISet<string> GetStopWords()
        {
            return _indexer.GetStopWords();
        }

        public Analyzer GetAnalyzer()
        {
            return _indexer.GetAnalyzer();
        }
    }
}