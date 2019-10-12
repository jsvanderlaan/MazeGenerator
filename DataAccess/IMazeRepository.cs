using Common;
using DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IMazeRepository
    {
        Task StoreMaze(MazeDto entity, List<ImageDto> images, List<Timer> timers);
    }
}