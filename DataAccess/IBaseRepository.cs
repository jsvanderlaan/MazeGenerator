using Common;
using DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IBaseRepository
    {
        Task Store(object obj);
        Task Store(MazeDto maze, List<ImageDto> images, List<Timer> timers);
    }
}