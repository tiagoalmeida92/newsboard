using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBoard.Utils;

namespace NewsBoard.Indexer.Utils
{

    /// <summary>
    /// Singleton class to get the stop words file.
    /// This stop words set will be created and filled, only the first time that is called the class constructor
    /// </summary>
    public sealed class PortugueseStopWords
    {
        private static volatile PortugueseStopWords instance;
        private static ISet<string> stopWords;
        private static readonly object _lock = new object();

        private PortugueseStopWords()
        {
            Files.AsyncReadStopWordsToHashSet(Constants.Constants.PATHSTOPWORDS,
                Constants.Constants.BLOBSTOPWORDS,
                Constants.Constants.AZURECONSTRING)
                .ContinueWith(task => { stopWords = task.Result; }, TaskContinuationOptions.None);
        }

        public static PortugueseStopWords Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                            instance = new PortugueseStopWords();
                    }
                }
                return instance;
            }
        }

        public ISet<String> GetStopWords()
        {
            return stopWords;
        }
    }
}