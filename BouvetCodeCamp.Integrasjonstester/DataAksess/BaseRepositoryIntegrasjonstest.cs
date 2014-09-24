namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Configuration;

    using BouvetCodeCamp.Dataaksess;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract class BaseRepositoryIntegrasjonstest
    {
        private readonly string databaseId;
        private readonly string endpoint;
        private readonly string authKey;

        protected BaseRepositoryIntegrasjonstest()
        {
            this.databaseId = ConfigurationManager.AppSettings[DocumentDbKonstanter.DatabaseId];
            endpoint = ConfigurationManager.AppSettings[DocumentDbKonstanter.Endpoint];
            authKey = ConfigurationManager.AppSettings[DocumentDbKonstanter.AuthKey];
        }
        
        [TestInitialize]
        public async void F�rHverTest()
        {
            using (var client = new DocumentClient(new Uri(endpoint), authKey))
            {
                DocumentDbHelpers.SlettDatabaseAsync(client, this.databaseId).RunSynchronously();

                DocumentDbHelpers.HentEllerOpprettDatabaseAsync(client, databaseId).RunSynchronously();
            }
        }
    }
}