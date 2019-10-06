using System.Threading.Tasks;

namespace DataAccess
{
    public interface IBaseRepository
    {
        Task Store(object obj);
    }
}