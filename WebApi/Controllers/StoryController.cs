using DataAccess;
using Entities.Story;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/story")]
    public class StoryController : ControllerBase
    {
        private readonly IStoryRepository _repo;

        public StoryController(IStoryRepository repo) 
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<List<Story>> Get() => await _repo.GetStories();

        [HttpPost]
        public async Task Post([FromBody] Story story) => await _repo.StoreStory(story);
    }
}
