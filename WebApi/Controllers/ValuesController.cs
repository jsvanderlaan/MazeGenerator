using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/values")]
    public class ValuesController : Controller
    {
        private string _value = "joooohos";

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return _value;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"jos{id}";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            _value = value;
        }
    }
}
