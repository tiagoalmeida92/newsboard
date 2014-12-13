using System;
using System.Collections.Generic;
using System.Linq;
using NewsBoard.Indexer.Model;

namespace NewsBoard.Indexer.Utils
{
    /// <summary>
    ///     Distinguish words and update dictionaries where that words appear (frequency and documentCount)
    ///     This class was made because Lucene doesn't allows DISTINCT
    /// </summary>
    public class WordsDistinguisher
    {
        public static void DistinguishWords(string[] termList, int[] freqList,
            ref Dictionary<string, FrequencyAndDocsCount> frequencyDic)
        {
            int nrTerms = termList.Length;
            for (int i = 0; i < nrTerms; ++i)
            {
                if (frequencyDic.ContainsKey(termList[i]))
                {
                    frequencyDic[termList[i]].UpdateFreqAndDocCount(freqList[i]);
                }
                else
                {
                    frequencyDic.Add(termList[i], new FrequencyAndDocsCount(freqList[i]));
                }
            }
        }

        /// <summary>
        ///     Order the frequencies and words.
        ///     Get top words in each news.
        /// </summary>
        /// <param name="termList">Array of words</param>
        /// <param name="freqList">Array of frequencies</param>
        /// <param name="frequencyDic"></param>
        /// <param name="topTermLimit">percentage limit to top term</param>
        public static void GetDocsTopWords(string[] termList, int[] freqList,
            ref Dictionary<string, FrequencyAndDocsCount> frequencyDic, float topTermLimit = 0.8F)
        {
            float topFreq = -0.1F;
            int nrTerms = termList.Length;
            Array.Sort(freqList, termList, new DescendingComparer());
            for (int i = 0; i < nrTerms; ++i)
            {
                if (topFreq < 0.0F)
                {
                    topFreq = freqList[i];
                    if (frequencyDic.ContainsKey(termList[i]))
                    {
                        frequencyDic[termList[i]].UpdateFreqAndDocCount(freqList[i]);
                    }
                    else
                    {
                        frequencyDic.Add(termList[i], new FrequencyAndDocsCount(freqList[i]));
                    }
                }
                else
                {
                    float ratio = freqList[i]/topFreq;
                    if (ratio >= topTermLimit)
                    {
                        if (frequencyDic.ContainsKey(termList[i]))
                        {
                            frequencyDic[termList[i]].UpdateFreqAndDocCount(freqList[i]);
                        }
                        else
                        {
                            frequencyDic.Add(termList[i], new FrequencyAndDocsCount(freqList[i]));
                        }
                    }
                    else break;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ordered"></param>
        /// <param name="list"></param>
        /// <param name="topTermLimit"></param>
        public static void GetTopWords(IOrderedEnumerable<KeyValuePair<string, FrequencyAndDocsCount>> ordered,
            List<string> list, float topTermLimit = 0.8F)
        {
            float topFreq = -0.1F;
            foreach (var keyValuePair in ordered)
            {
                if (topFreq < 0.0F)
                {
                    topFreq = keyValuePair.Value.GetDocFreq();
                    list.Add(keyValuePair.Key);
                }
                else
                {
                    float ratio = keyValuePair.Value.GetDocFreq()/topFreq;
                    if (ratio >= topTermLimit)
                    {
                        list.Add(keyValuePair.Key);
                    }
                    else break;
                }
            }
        }
    }
}