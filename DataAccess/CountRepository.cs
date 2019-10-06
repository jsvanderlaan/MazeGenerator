using Entities.Counter;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CountRepository : ICountRepository
    {
        private readonly IBaseRepository _baseRepository;

        public CountRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task StoreCount(ICount count) => await _baseRepository.Store(count); 
    }
}
