using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Entities.Converters;
using System;
using Entities.Mazes;
using System.Drawing;
using System.Drawing.Imaging;
using DataAccess;
using System.Collections.Generic;
using DataTransferObjects.Mappers;
using DataTransferObjects;
using Common.Enums;
using Common.Extensions;
using Common;

namespace WebApi.Controllers
{
    [Route("api/FileUpload")]
    [Produces("application/json")]
    public class FileUploadController : Controller
    {
        private readonly Shape _shape = Shape.Hexagonal;
        private readonly IMazeRepository _mazeRepository;
        private readonly MazeMapper _mapper;

        public FileUploadController(IMazeRepository repo)
        {
            _mazeRepository = repo;
            _mapper = new MazeMapper();
        }

        [HttpPost]
        public async Task<string> Post()
        {
            //var imageToGridParallelTimer = new Timer(TimerCategory.Maze, TimerTask.ImageToGrid, TimerAction.Parallel);
            var imageToGridTimer = new Timer(TimerCategory.Maze, TimerTask.ImageToGrid, TimerAction.Synchroon);
            var generationTimer = new Timer(TimerCategory.Maze, TimerTask.GenerateMaze);
            var mazeToImageTimer = new Timer(TimerCategory.Maze, TimerTask.MazeToImage, TimerAction.WithoutSolution);
            var mazeToSolutionImageTimer = new Timer(TimerCategory.Maze, TimerTask.MazeToImage, TimerAction.WithSolution);

            var request = await HttpContext.Request.ReadFormAsync();
            var formFile = request.Files?[0];
            if (formFile == null) throw new FileNotFoundException();

            string response;
            using (var original = Image.FromStream(formFile.OpenReadStream()))
            {
                imageToGridTimer.Start();
                var grid = ImageToGridConverter.Convert(new Bitmap(original), 100, 0.5, true, _shape);
                imageToGridTimer.Stop();
                //imageToGridParallelTimer.Start();
                //grid =// ImageToGridConverter.ConvertParallel(new Bitmap(original), 100, 0.5, true, _shape);
                //imageToGridParallelTimer.Stop();

                generationTimer.Start();
                var maze = new Maze(grid, _shape, true).GenerateWithRandomList();
                generationTimer.Stop();

                var imageConverter = new MazeToImageConverter(maze);
                var solutionConverter = new MazeToImageConverter(maze);
                using (var imageStream = new MemoryStream())
                using (var solutionStream = new MemoryStream())
                using (var originalStream = new MemoryStream())
                {
                    mazeToImageTimer.Start();
                    using (var image = imageConverter.GetMaze(false))
                    {
                        mazeToImageTimer.Stop();
                        mazeToSolutionImageTimer.Start();
                        using (var solution = solutionConverter.GetMaze(true))
                        {
                            mazeToSolutionImageTimer.Stop();
                            image.Save(imageStream, ImageFormat.Png);
                            solution.Save(solutionStream, ImageFormat.Png);
                            original.Save(originalStream, original.RawFormat);
                            originalStream.Position = 0;
                            imageStream.Position = 0;
                            solutionStream.Position = 0;
                            var name = Path.GetFileNameWithoutExtension(formFile.FileName);
                            var extension = Path.GetExtension(formFile.FileName);
                            var images = new List<ImageDto>
                            {
                                new ImageDto { Data = originalStream, Name = $"{name}{extension}", ContentType = original.RawFormat.GetMimeType() },
                                new ImageDto { Data = imageStream, Name = $"{name}_maze.png", ContentType = ImageFormat.Png.GetMimeType() },
                                new ImageDto { Data = solutionStream, Name = $"{name}_solution.png", ContentType = ImageFormat.Png.GetMimeType() },
                            };
                            var timers = new List<Timer>
                            {
                                imageToGridTimer,
                                generationTimer,
                                mazeToSolutionImageTimer,
                                mazeToImageTimer,
                                //imageToGridParallelTimer
                            };
                            await _mazeRepository.StoreMaze(_mapper.Map(maze), images, timers);
                            response = Convert.ToBase64String(imageStream.ToArray());
                        }
                    }
                }
            }

            return response;
        }
    }
}