using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.BR;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using NewsBoard.Indexer.Utils;
using NewsBoard.Utils;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace NewsBoard.Indexer
{
    public abstract class Indexer<T> : IIndexer<T>
    {
        protected readonly Directory _dir;
        private readonly String _storageType;

        private Analyzer _analyzer;
        private HtmlArticleExtraction _extractor;
        private IndexReader _reader;
        private PortugueseStopWords _stopWordsObj;
        private string _systemOrCloudPath;

        //Individual Index
        private IndexWriter _writer;

        /// <summary>
        ///     Indexer is a class that will take advantage of funcionalities of the Lucene to index files
        /// </summary>
        protected Indexer()
        {
            _storageType = Constants.Constants.DEFAULTSTORAGETYPE;
            _systemOrCloudPath = Constants.Constants.PATHALLWORDSTOINDEX;
            _stopWordsObj = PortugueseStopWords.Instance;

            if ((_dir = GetDirectory(_systemOrCloudPath)) == null)
            {
                throw new DirectoryNotFoundException();
            }
        }

        /// <summary>
        ///     Get the identifiers of the documents
        /// </summary>
        /// <param name="fieldName">Name of the field(indexed) to get documents</param>
        /// <param name="value">Value of the field(indexed) to get documents</param>
        /// <returns>Enumerable of document identifiers</returns>
        public IEnumerable<int> GetDocumentsIds(String fieldName, String value)
        {
            IndexReader reader = GetReader();
            var searcher = new IndexSearcher(reader);
            var booleanQuery = new BooleanQuery();
            Query query1 = new TermQuery(new Term(fieldName, value));
            booleanQuery.Add(query1, Occur.SHOULD);
            var collector = new WithoutScoreCollector();
            searcher.Search(booleanQuery, collector);
            return collector.DocumentIds;
        }

        /// <summary>
        /// Update a document Field
        /// </summary>
        /// <param name="manualCategoryField">Manual Category Field Name</param>
        /// <param name="manualCategoryValue">Manual Category Field Value</param>
        /// <param name="docId">Lucene Document identifier</param>
        public void UpdateDocField(string manualCategoryField, string manualCategoryValue, int docId)
        {
            IndexReader reader = GetReader();
            Document doc = reader.Document(docId);
            doc.RemoveField(Constants.Constants.MANUAL_CATEGORY_FIELD);
            doc.Add(new Field(Constants.Constants.MANUAL_CATEGORY_FIELD, manualCategoryValue, Field.Store.YES,
                Field.Index.NOT_ANALYZED, Field.TermVector.YES));
            string link = doc.GetField(Constants.Constants.LINK_FIELD).StringValue;
            _writer.UpdateDocument(new Term(Constants.Constants.LINK_FIELD, link), doc);
        }

        /// <summary>
        ///     Get the directory.
        /// </summary>
        /// <returns>Lucene.Net.Store.Directory instance</returns>
        public Directory GetDirectory()
        {
            return _dir;
        }

        /// <summary>
        /// Get an IndexWriter to write Lucene documents
        /// </summary>
        /// <returns>IndexWriter instance</returns>
        public IndexWriter GetWriter()
        {
            return _writer;
        }

        /// <summary>
        /// method to define analyzers, the directory 
        /// and the IndexWriter instance (instance used 
        /// needed to write file Lucene
        /// </summary>
        public void InstantiateWriter()
        {
            bool isEmptyDir = !IndexReader.IndexExists(_dir);
            Analyzer analyzer = new FirstUpperPortugueseAnalyzer(Version.LUCENE_30, GetStopWords());
            Analyzer synonymAnalyzer = new OntoPtAnalyzer(Version.LUCENE_30, GetStopWords());
            Analyzer secondAnalyzer = new BrazilianAnalyzer(Version.LUCENE_30, GetStopWords());
            var perFieldAnalyzer = new PerFieldAnalyzerWrapper(analyzer);
            perFieldAnalyzer.AddAnalyzer(Constants.Constants.TITLE_FIELD, synonymAnalyzer);
            perFieldAnalyzer.AddAnalyzer(Constants.Constants.CONTENT_TO_SEARCH_FIELD, secondAnalyzer);
            _writer = new IndexWriter(_dir, perFieldAnalyzer, isEmptyDir, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        /// <summary>
        /// Method to Write a Lucene index and unlock the 'write lock' (thread-safe).
        /// </summary>
        public void WriteAndDispose()
        {
            try
            {
                if (_writer != null)
                {
                    _writer.Optimize();
                    _writer.Dispose();
                }
            }
            finally
            {
                if (IndexWriter.IsLocked(_dir))
                {
                    IndexWriter.Unlock(_dir);
                }
            }
        }

        /// <summary>
        /// Method to set the instance of HtmlArticleExtraction to be called on Scraper.cs
        /// This instance need to be set because of the method Index (from NewsIndexer.cs) 
        /// need to know the extrator to obtain the article and index
        /// </summary>
        /// <param name="extractor">Instance of HtmlArticleExtraction</param>
        public void SetHtmlExtractor(HtmlArticleExtraction extractor)
        {
            _extractor = extractor;
        }

        /// <summary>
        /// Method to Get the instance of HtmlExtractor. Called on Index of NewsIndexer class
        /// </summary>
        /// <returns></returns>
        public HtmlArticleExtraction GetHtmlExtractor()
        {
            return _extractor;
        }

        /// <summary>
        ///     Get the IndexReader
        /// </summary>
        /// <returns>IndexReader instance</returns>
        public IndexReader GetReader()
        {
            return _reader != null && _reader.IsCurrent() ? _reader : (_reader = IndexReader.Open(_dir, true));
        }

        /// <summary>
        ///     Get the stop words to being removed from the article,
        ///     from the rss source
        /// </summary>
        /// <returns>Set of words to remove</returns>
        public ISet<String> GetStopWords()
        {
            return _stopWordsObj.GetStopWords();
        }

        /// <summary>
        /// Instance to be analyzed
        /// </summary>
        /// <returns></returns>
        public Analyzer GetAnalyzer()
        {
            return _analyzer ?? (_analyzer = new BrazilianAnalyzer(Version.LUCENE_30, _stopWordsObj.GetStopWords()));
        }

        /// <summary>
        /// Method to instantiate Fields, to add Fields to a document, and add documents to a IndexWriter instance.
        /// IndexWriter instance that will write the documents to lucene files/lucene indexes.
        /// </summary>
        /// <param name="item">Type/News to be indexed and added to IndexWriter instance</param>
        /// <param name="createNew">Flag to indicate if the directory needs to be created</param>
        public abstract void Index(T item, bool createNew);

        /// <summary>
        ///     Get the directory. Storage type was passed at constructor
        /// </summary>
        /// <returns>Lucene.Net.Store.Directory instance</returns>
        /// <param name="systemOrCloudPath">Path of the directory returned</param>
        private Directory GetDirectory(String systemOrCloudPath)
        {
            Func<String, Directory> func;
            return Constants.Constants.DIRECTORYSTORAGE.TryGetValue(_storageType, out func)
                ? func(systemOrCloudPath)
                : null;
        }
    }
}