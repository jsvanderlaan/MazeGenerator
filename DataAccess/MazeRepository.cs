using Common;
using DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MazeRepository : BaseRepository, IMazeRepository
    {
        public MazeRepository() : base("Mazes")
        {
        }

        public async Task StoreMaze(MazeDto entity, List<ImageDto> images, List<Timer> timers)
        {
            await Store(entity, images, timers);
        }
    }
}
