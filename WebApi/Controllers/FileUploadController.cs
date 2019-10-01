using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Entities.Converters;
using Common;
using System;
using Entities.Mazes;
using Microsoft.AspNetCore.Hosting;

namespace WebApi.Controllers
{
    [Route("api/FileUpload")]
    [Produces("application/json")]
    public class FileUploadController : Controller
    {
        private readonly Shape _shape = Shape.Hexagonal;
        private readonly IHostingEnvironment _env;

        public FileUploadController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<string> Post()
        {
            var guid = Guid.NewGuid();
            var request = await HttpContext.Request.ReadFormAsync();
            var formFile = request.Files?[0];
            if (formFile == null) throw new FileNotFoundException();

            Boolean[,] grid;

            var dir = Path.Combine(_env.WebRootPath, "results");
            var extension = Path.GetExtension(formFile.FileName);
            Directory.CreateDirectory(dir);
            using (var fileStream = new FileStream(Path.Combine(dir, $"{guid}_original{extension}"), FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                var type = Path.GetExtension(formFile.FileName);
                grid = ImageToGridConverter.Convert(memoryStream, 100, 0.5, true, _shape);
            }

            var maze = new Maze(grid, _shape, true);
            maze.GenerateWithRandomList(maze.StartingCell.Position);
            var converter = new MazeToImageConverter(maze);
            var result = converter.GetMaze(false, Path.Combine(dir, $"{guid}_maze{extension}"));
            converter = new MazeToImageConverter(maze);
            converter.GetMaze(true, Path.Combine(dir, $"{guid}_solution{extension}"));
            var base64 = Convert.ToBase64String(result);
            return base64; // new ResponseFile { Type = "image/png", Base64 = base64 };
            //$"{{\"type\": \"image/png\", \"base64\": \"{base64}\""
        }
    }

    public class ResponseFile
    {
        public string Type { get; set; }
        public string Base64 { get; set; }
    }
}