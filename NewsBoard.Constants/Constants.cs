using Lucene.Net.Store;
using Lucene.Net.Store.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using Directory = Lucene.Net.Store.Directory;

namespace NewsBoard.Constants
{
    public class Constants
    {
        public static readonly string DEFAULTSTORAGETYPE =
            ConfigurationManager.AppSettings[Environment.MachineName] != null ? SYSTEMSTORAGE : AZURESTORAGE;

        /*** PRIVATE ***/

        //Directory where ontopt will go
        private static readonly String SYSTEMONTOPTPATH = ConfigurationManager.AppSettings[Environment.MachineName] +
                                                    @"\OntoPTv0.6.n3";

        private const String AZUREONTOPTCATALOG = "firstcontainer";

        //Directory where indexes will go
        private static readonly String SYSTEMALLWORDSPATH = ConfigurationManager.AppSettings[Environment.MachineName] +
                                                            @"\AllWordsIndexes";

        private const String AZUREALLWORDSCATALOG = "allwordsluceneindex";

        //Directory where stop words will go
        private static readonly String SYSTEMSTOPWORDPATH = ConfigurationManager.AppSettings[Environment.MachineName] +
                                                            @"\lista de stopwords Portugues.txt";

        private const String AZURESTOPWORDSCATALOG = "firstcontainer";

        //Blob stop words name
        private const String AZURESTOPWORDSBLOCKBLOB = "StopWords.txt";

        public static string BLOBSTOPWORDS = DEFAULTSTORAGETYPE == AZURESTORAGE ? AZURESTOPWORDSBLOCKBLOB : null;

        //Blob ontopt name
        private const String AZUREONTOPTBLOCKBLOB = "OntoPTv0.6.n3";

        public static string BLOBONTOPT = DEFAULTSTORAGETYPE == AZURESTORAGE ? AZUREONTOPTBLOCKBLOB : null;

        //Storage Type
        private const String SYSTEMSTORAGE = "System";

        private const String AZURESTORAGE = "Azure";

        /***PUBLIC***/

        // Scraper Constants
        public const int SleepMillis = 5 * 60 * 1000; //5 min

        public const String DEFAULTMANUALCATEGORY = "Indiferenciada";

        // Rss Indexer Constants
        public const String LINK_FIELD = "Link";

        public const String CATEGORY_FIELD = "Category";
        public const String MANUAL_CATEGORY_FIELD = "Manual Category";
        public const String TITLE_FIELD = "Title";
        public const String ARTICLE_FIELD = "Article";
        public const String ADMINSUBJECTS_FIELD = "Admin Subjects";
        public const String CONTENT_TO_SEARCH_FIELD = "Content to search";

        //Directories
        public static readonly String PATHALLWORDSTOINDEX = DEFAULTSTORAGETYPE == AZURESTORAGE ? AZUREALLWORDSCATALOG : SYSTEMALLWORDSPATH;

        public static readonly String PATHSTOPWORDS = DEFAULTSTORAGETYPE == AZURESTORAGE ? AZURESTOPWORDSCATALOG : SYSTEMSTOPWORDPATH;

        public static readonly String PATHONTOPT = DEFAULTSTORAGETYPE == AZURESTORAGE ? AZUREONTOPTCATALOG : SYSTEMONTOPTPATH;

        //Storage type
        //public const String DEFAULTSTORAGETYPE = SYSTEMSTORAGE;
                                                //AZURESTORAGE;
        public const string AZURECONSTRING = "StorageConnectionString";

        //Functions to get the directory instance
        public static readonly Dictionary<String, Func<String, Directory>> DIRECTORYSTORAGE = new Dictionary<String, Func<String, Directory>>()
        {
            {SYSTEMSTORAGE, (indexName)=> { return FSDirectory.Open(indexName); }},
            {AZURESTORAGE, (indexName) =>
                            {
                                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(

                                    CloudConfigurationManager.GetSetting(AZURECONSTRING));

                                var cacheDirectory = new RAMDirectory();

                                //const string indexName = "MyLuceneIndex";
                                return new AzureDirectory(storageAccount, indexName, cacheDirectory);
                            }}
        };

        //Constants to calculate the percentage of category in a news (total = 1.0F)
        //Admin words = 20%
        public const float PERCENTAGE_CATEGORY_ADMINWORDS = 0.3F;

        //Title words = 20%
        public const float PERCENTAGE_CATEGORY_TITLEWORDS = 0.2F;

        //Article words = 60%
        public const float PERCENTAGE_CATEGORY_ARTICLEWORDS = 0.5F;

        //Probability constant to associate categories with news
        //1%
        public const float PERCENTAGE_WHEN_ASSOCIATE_CATEGORY_TO_NEWS = 0.02F;

        public const float PERCENTAGE_LIMIT_TO_CONDITION_TO_ASSOCIATE = 0.01F;

        /*** Facebook Authentication, Likes And Posts ***/

        public const string FACEBOOK_ACCESS_TOKEN_CLAIM_TYPE = "urn:facebook:access_token"; 

    }
}