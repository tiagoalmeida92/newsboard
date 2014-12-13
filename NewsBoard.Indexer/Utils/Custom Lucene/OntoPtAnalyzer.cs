using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace NewsBoard.Indexer.Utils
{
    /// <summary>
    /// Lucene analyzer to call a service (instantiate OntoPtFilter) to get the synonyms and hypernyms
    /// </summary>
    public class OntoPtAnalyzer : StandardAnalyzer
    {
        private readonly ISet<string> _stopWords;
        private readonly Version _version;

        public OntoPtAnalyzer(Version version, ISet<string> stopWords)
            : base(version, stopWords)
        {
            _version = version;
            _stopWords = stopWords;
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            var tokenizer = new StandardTokenizer(_version, reader);
            TokenStream tokenStream = new StandardFilter(tokenizer);
            tokenStream = new StopFilter(true, tokenStream, _stopWords, true);
            tokenStream = new LowerCaseFilter(tokenStream);
            tokenStream = new OntoPtFilter(tokenStream);
            return tokenStream;
        }
    }
}