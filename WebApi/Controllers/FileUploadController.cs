using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/FileUpload")]
    public class FileUploadController : Controller
    {
        [HttpPost]
        public async Task Post()
        {
            var request = await HttpContext.Request.ReadFormAsync();
            return;
        }
    }
}