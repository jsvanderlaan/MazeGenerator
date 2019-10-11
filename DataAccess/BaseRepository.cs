using DataTransferObjects;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Embedded;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IDocumentStore _store;

        public BaseRepository(string documentStore)
        {
            _store = EmbeddedServer.Instance.GetDocumentStore(documentStore);
        }

        public async Task Store(object obj)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                await session.StoreAsync(obj);
                await session.SaveChangesAsync();
            }
        }

        public async Task Store(object obj, List<ImageDto> images)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                await session.StoreAsync(obj);
                var id = session.Advanced.GetDocumentId(obj);
                images.ForEach(image => session.Advanced.Attachments.Store(obj, image.Name, image.Data, image.ContentType));
                await session.SaveChangesAsync();
            }
        }
    }
}
