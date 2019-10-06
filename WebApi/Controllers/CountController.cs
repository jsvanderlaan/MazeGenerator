using DataAccess;
using Entities.Counter;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/count")]
    public class CountController : Controller
    {
        private readonly ICountRepository _countRepository;
        public CountController(ICountRepository countRepository)
        {
            _countRepository = countRepository;
        }

        [HttpGet]
        [Route("click/{name}")]
        public async Task CountClick(string name)
        {
            ICount count = new ClickCount(name);
            await _countRepository.StoreCount(count);
        }
    }
}
