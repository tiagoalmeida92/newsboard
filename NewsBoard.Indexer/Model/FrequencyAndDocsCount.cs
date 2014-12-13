namespace NewsBoard.Indexer.Model
{
    /// <summary>
    /// Class used only on NewsBoard.Indexer project
    /// Type to aggregate and incremente the documents number and number of words frequency
    /// </summary>
    public class FrequencyAndDocsCount
    {
        private int _docCount;
        private int _docFreq;

        public FrequencyAndDocsCount(int docFreq)
        {
            _docFreq = docFreq;
            _docCount = 1;
        }

        public int GetDocCount()
        {
            return _docCount;
        }

        public int GetDocFreq()
        {
            return _docFreq;
        }

        public void UpdateFreqAndDocCount(int moreFreq)
        {
            _docFreq += moreFreq;
            _docCount += 1;
        }
    }
}