using Entities.Counter;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CountRepository : BaseRepository, ICountRepository
    {

        public CountRepository() : base("Counts")
        {
        }

        public async Task StoreCount(ICount count) => await Store(count); 
    }
}
