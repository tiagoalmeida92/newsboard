using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace NewsBoard.Utils
{
    public static class Files
    {
        /// <summary>
        ///     Method to read a file of stopwords to HashSet
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        /// <returns></returns>
        public static async Task<ISet<string>> AsyncReadStopWordsToHashSet(string filePath, string blob,
            string conString)
        {
            StreamReader sourceStream;
            //Enconding code=1252 - Latin Western Europe
            Encoding encoding = Encoding.GetEncoding(1252);
            if (blob != null)
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting(conString));

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(filePath);
                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob);
                sourceStream = new StreamReader(blockBlob.OpenRead(), encoding);
            }
            else
            {
                sourceStream = new StreamReader(filePath, encoding);
            }
            var strings = new HashSet<String>();
            using (sourceStream)
            {
                String text;
                while ((text = await sourceStream.ReadLineAsync()) != null)
                {
                    strings.Add(text.Replace(Environment.NewLine, string.Empty));
                }

                return strings;
            }
        }

        /// <summary>
        /// Reads the OntoPT file asynchronously into an object graph
        /// </summary>
        /// <returns></returns>
        public static async Task<Graph> AsyncReadOntoPtToDictionary(string filePath, string blob, string conString)
        {
            TextReader sourceStream;
            if (blob != null)
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting(conString));

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(filePath);
                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob);
                sourceStream = new StreamReader(blockBlob.OpenRead());
            }
            else
            {
                sourceStream = new StreamReader(filePath);
            }
            var parser = new Notation3Parser();
            var graph = new Graph();
            parser.Load(graph, sourceStream);
            return graph;
        }
    }
}