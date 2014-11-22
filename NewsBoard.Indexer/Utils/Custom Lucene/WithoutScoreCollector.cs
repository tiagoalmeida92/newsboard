using System;
using System.Collections.Generic;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace NewsBoard.Indexer.Utils
{
    /// <summary>
    /// Simple collector for Lucene querys to get the list of documents 
    /// resulting from a search (without the filter by score (Lucene type ScoreDocs))
    /// </summary>
    public class WithoutScoreCollector : Collector
    {
        private readonly List<Int32> _docIds = new List<Int32>();
        private Int32 _docBase;

        public IEnumerable<Int32> DocumentIds
        {
            get { return _docIds; }
        }

        public override bool AcceptsDocsOutOfOrder
        {
            get { return true; }
        }

        public override void SetScorer(Scorer scorer)
        {
        }

        public override void Collect(Int32 doc)
        {
            _docIds.Add(_docBase + doc);
        }

        public override void SetNextReader(IndexReader reader, Int32 docBase)
        {
            _docBase = docBase;
        }
    }
}