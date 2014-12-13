using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;

namespace NewsBoard.Indexer.Utils
{
    /// <summary>
    ///     Custom OntoPT Filter
    ///     Filter to call the OntoPtService and get synonyms and hypernyms for specific words
    ///     Reference: http://www.opten.ch/blog/2014/05/30/writing-a-custom-synonym-token-filter-in-lucenenet/
    /// </summary>
    public sealed class OntoPtFilter : TokenFilter
    {
        private State _currentState;
        private Stack<string> _currentSynonymsOrHypernyms;
        private OntoPtService _ontoPtService;
        private PositionIncrementAttribute _posAtt;
        private ITermAttribute _termAtt;

        public OntoPtFilter(TokenStream input)
            : base(input)
        {
            _termAtt = AddAttribute<ITermAttribute>();
            _posAtt = (PositionIncrementAttribute) AddAttribute<IPositionIncrementAttribute>();
            _currentSynonymsOrHypernyms = new Stack<string>();
            _ontoPtService = OntoPtService.Instance;
        }

        public override bool IncrementToken()
        {
            if (_currentSynonymsOrHypernyms.Count > 0)
            {
                string synonym = _currentSynonymsOrHypernyms.Pop();
                RestoreState(_currentState);
                _termAtt.SetTermBuffer(synonym);
                _posAtt.PositionIncrement = 0;
                return true;
            }

            if (!input.IncrementToken()) return false;
            //get current term
            string curr = _termAtt.Term;
            string currTerm = curr.EndsWith("s") ? curr.Substring(0, curr.Length - 1) : curr;

            if (currTerm != null)
            {
                IList<string> synonyms = _ontoPtService.GetSynonymsAndHypernyms(currTerm);
                if (!synonyms.Any()) return true;
                foreach (string synonym in synonyms)
                {
                    _currentSynonymsOrHypernyms.Push(synonym);
                }
            }
            _currentState = CaptureState();
            return true;
        }
    }
}