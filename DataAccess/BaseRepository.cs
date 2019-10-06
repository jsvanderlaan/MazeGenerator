using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Embedded;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IDocumentStore _store;

        public BaseRepository(DatabaseOptions options)
        {
            ServerOptions serverOptions = new ServerOptions
            {
                DataDirectory = @"D:\Projecten HDD\RavenDb\MazeGenerator",
                ServerUrl = "http://127.0.0.1:8080",
                FrameworkVersion = "2.2.7"
            };
            EmbeddedServer.Instance.StartServer(serverOptions);
            _store = EmbeddedServer.Instance.GetDocumentStore("BramdaDb");
        }

        public async Task Store(object obj)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                await session.StoreAsync(obj);
                await session.SaveChangesAsync();
            }
        }
    }
}
