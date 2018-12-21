using MazeGenerator;
using MazeGenerator.Cells;
using MazeGenerator.Drawing;
using MazeGenerator.ImageConverter;
using MazeGenerator.Mazes;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace MazeGeneration
{
    public static class Program
    {
        static string file;
        static string outputDir;
        static string inputDir;
        static string outputFileType;
        static int mazeHeight;
        static double minimalBrightness;
        static int numberOfMazes;

        static void Main(string[] args)
        {
            Configure();

            var filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + inputDir + "\\" + file;
            var grid = file == "" ? new bool[0,0] : ImageToGridConverter<HexagonalCell>.Convert(filePath, mazeHeight, minimalBrightness);

            for ( int i = 1; i <= numberOfMazes; i++)
            {
                var outputPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + outputDir + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + i.ToString("D3") + outputFileType;
                var outputPathAnswer = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + outputDir + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + i.ToString("D3") + "answer" + outputFileType;

                var maze = file == "" ? new Maze<HexagonalCell>(mazeHeight, mazeHeight) : new Maze<HexagonalCell> (grid);

                maze.GenerateWithStack(maze.StartingCell.Position);
                //maze.GenerateWithRandomList(maze.StartingCell.Position);

                var converter = new MazeToImageConverter<HexagonalCell>(maze);
                converter.SaveMaze(outputPath, false);
                converter.SaveMaze(outputPathAnswer, true);
            }

            //Console.ReadKey();
        }

        static void Configure()
        {
            var config = ConfigurationManager.GetSection("Program") as NameValueCollection;
            file = config["file"];
            outputDir = config["outputDir"];
            inputDir = config["inputDir"];
            outputFileType = config["outputFileType"];
            mazeHeight = Int32.Parse(config["mazeHeight"]);
            minimalBrightness = Double.Parse(config["minimalBrightness"]);
            numberOfMazes = Int32.Parse(config["numberOfMazes"]);
        }
    }
}