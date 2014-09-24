using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    using Microsoft.Azure.Documents.Client;

    public abstract class Repository<T> : IRepository<T> where T : BaseDocument
    {
        public abstract string CollectionId { get; }
        
        protected readonly IKonfigurasjon _konfigurasjon;
        protected readonly IDocumentDbContext Context;

        private DocumentCollection _collection;
        public DocumentCollection Collection
        {
            get
            {
                if (_collection == null)
                {
                    _collection = Context.ReadOrCreateCollection(Context.Database.SelfLink, CollectionId);
                }

                return _collection;
            }
        }

        protected Repository(IKonfigurasjon konfigurasjon, IDocumentDbContext context)
        {
            _konfigurasjon = konfigurasjon;
            Context = context;
        }

        public async Task<string> Opprett(T document)
        {
            var opprettetDocument = await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);

            return opprettetDocument.Resource.Id;
        }

        public async Task<IEnumerable<T>> HentAlle()
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .AsEnumerable()
                    .ToList());
        }

        public async Task<T> Hent(string id)
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .Where(d => d.DocumentId == id)
                    .AsEnumerable()
                    .FirstOrDefault());
        }

        public async Task Oppdater(T document)
        {
            var entitet = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .Where(d => d.Id == document.DocumentId)
                .AsEnumerable()
                .FirstOrDefault();

            if (entitet == null)
                throw new Exception("Fant ikke entiteten som skulle oppdateres.");

            await Context.Client.ReplaceDocumentAsync(entitet.SelfLink, document);
        }

        public async Task Slett(T document)
        {
            var entitet = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .Where(d => d.Id == document.DocumentId)
                .AsEnumerable()
                .FirstOrDefault();

            if (entitet == null)
                throw new Exception("Fant ikke entiteten som skulle slettes.");

            await Context.Client.DeleteDocumentAsync(entitet.SelfLink, new RequestOptions());
        }
    }
}