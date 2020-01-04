using Entities.Story;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IStoryRepository
    {
        Task StoreStory(Story story);
        Task<List<Story>> GetStories();
    }
}