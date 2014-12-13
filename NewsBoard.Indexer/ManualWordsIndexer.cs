using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using NewsBoard.Indexer.Utils;
using NewsBoard.Model.Categories;
using Version = Lucene.Net.Util.Version;

namespace NewsBoard.Indexer
{
    public class ManualWordsIndexer : Indexer<UndifferentiatedCategory>
    {
        /// <summary>
        /// Method to Index/Update a Lucene already indexed document.
        /// Needed for manual categorization.
        /// </summary>
        /// <param name="item">Type to aggregate information about news that have undifferentiated category</param>
        /// <param name="createNew"></param>
        public override void Index(UndifferentiatedCategory item, bool createNew)
        {
            try
            {
                IndexReader reader = GetReader();
                IEnumerable<int> docsId = GetDocumentsIds(Constants.Constants.LINK_FIELD, item.Link);
                int docId = docsId.First();
                Analyzer analyzer = new FirstUpperPortugueseAnalyzer(Version.LUCENE_30, GetStopWords());
                Analyzer synonymAnalyzer = new OntoPtAnalyzer(Version.LUCENE_30, GetStopWords());
                var perFieldAnalyzer = new PerFieldAnalyzerWrapper(analyzer);
                perFieldAnalyzer.AddAnalyzer(Constants.Constants.TITLE_FIELD, synonymAnalyzer);
                var writer = new IndexWriter(GetDirectory(), perFieldAnalyzer, false,
                    IndexWriter.MaxFieldLength.UNLIMITED);
                Document doc = reader.Document(docId);
                doc.RemoveField(Constants.Constants.MANUAL_CATEGORY_FIELD);
                doc.RemoveField(Constants.Constants.ADMINSUBJECTS_FIELD);
                doc.Add(new Field(Constants.Constants.MANUAL_CATEGORY_FIELD, item.Category, Field.Store.YES,
                    Field.Index.NOT_ANALYZED, Field.TermVector.YES));
                doc.Add(new Field(Constants.Constants.ADMINSUBJECTS_FIELD, new StringReader(item.TopSubjectsWords),
                    Field.TermVector.WITH_POSITIONS));
                writer.UpdateDocument(new Term(Constants.Constants.LINK_FIELD, item.Link), doc);
                writer.Optimize();
                writer.Dispose();
            }
            finally
            {
                if (IndexWriter.IsLocked(_dir))
                {
                    IndexWriter.Unlock(_dir);
                }
            }
        }

    }
}