using Common;
using DataTransferObjects;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Embedded;
using System.Collections.Generic;
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

        public async Task Store(MazeDto maze, List<ImageDto> images, List<Timer> timers)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                timers.ForEach(timer => session.StoreAsync(timer));
                maze.Timers = timers.ConvertAll(timer => timer.Id);
                await session.StoreAsync(maze);
                images.ForEach(image => session.Advanced.Attachments.Store(maze, image.Name, image.Data, image.ContentType));
                await session.SaveChangesAsync();
            }
        }
    }
}
