using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace NewsBoard.Indexer.Utils
{

    /// <summary>
    /// Lucene analyzer to make the first letter of words to uppercase
    /// And removing portuguese stop words
    /// </summary>
    public class FirstUpperPortugueseAnalyzer : StandardAnalyzer
    {
        private readonly ISet<string> _stopWords;
        private readonly Version _version;

        public FirstUpperPortugueseAnalyzer(Version version, ISet<string> stopWords)
            : base(version, stopWords)
        {
            _version = version;
            _stopWords = stopWords;
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            var tokenizer = new StandardTokenizer(_version, reader);
            TokenStream filterStream = new StandardFilter(tokenizer);
            TokenStream stream = new StopFilter(true, filterStream, _stopWords, true);
            return stream;
        }
    }
}