using Entities.Story;
using Raven.Client.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class StoryRepository: BaseRepository, IStoryRepository
    {
        public StoryRepository() : base("Stories") { }

        public async Task StoreStory(Story story) => await Store(story);
        public async Task<List<Story>> GetStories() => await Get(session => session.Query<Story>().OrderByDescending(story => story.CreationTime).ToListAsync());
    }
}
