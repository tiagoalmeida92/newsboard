using System;
using System.Collections.Generic;
using NewsBoard.Utils;
using VDS.RDF;
using VDS.RDF.Query;

namespace NewsBoard.Indexer.Utils
{
    /// <summary>
    /// Singleton class to get the OntoPt file with several synonyms and hypernyms of several words.
    /// This OntoPt Graph will be created and filled, only the first time that is called the class constructor
    /// </summary>
    public sealed class OntoPtService
    {
        private static volatile OntoPtService instance;
        private static Graph ontoPtGraph;
        private static readonly object _lock = new object();

        private OntoPtService()
        {
            ontoPtGraph = Files.AsyncReadOntoPtToDictionary(Constants.Constants.PATHONTOPT,
                Constants.Constants.BLOBONTOPT,
                Constants.Constants.AZURECONSTRING).Result;
        }

        public static OntoPtService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                            instance = new OntoPtService();
                    }
                }
                return instance;
            }
        }

        public IList<string> GetSynonymsAndHypernyms(string currTerm)
        {
            //Starting from a term remove synonyms and hypernyms
            Object results = ontoPtGraph.ExecuteQuery(@"PREFIX OntoPT: <http://ontopt.dei.uc.pt/OntoPT.owl#>" +
                                                      " SELECT DISTINCT ?word " +
                                                      "WHERE " +
                                                      "{ ?x OntoPT:formaLexical \"" + currTerm +
                                                      "\" ; OntoPT:formaLexical ?word " +
                                                      "}");
            var synonyms = new List<string>();
            const string WORD = "word";
            if (results is SparqlResultSet)
            {
                //Print out the Results
                var rset = (SparqlResultSet) results;
                var aux = new List<string>();
                foreach (SparqlResult result in rset)
                {
                    synonyms.Add(result.Value(WORD).ToString());
                }
            }

            return synonyms;
        }
    }
}