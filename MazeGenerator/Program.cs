using MazeGenerator;
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
            var grid = ImageToGridConverter.Convert(filePath, mazeHeight, minimalBrightness);

            for( int i = 1; i <= numberOfMazes; i++)
            {
                var outputPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + outputDir + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + i.ToString("D3") + outputFileType;
                var outputPathAnswer = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + outputDir + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + i.ToString("D3") + "answer" + outputFileType;

                var maze = new GraphicalMaze(grid);
                maze.DrawMaze(outputPath, false);
                maze.DrawMaze(outputPathAnswer, true);
            }

            Console.ReadKey();
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