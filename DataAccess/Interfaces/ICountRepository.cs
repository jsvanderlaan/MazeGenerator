using Entities.Counter;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ICountRepository
    {
        Task StoreCount(ICount count);
    }
}