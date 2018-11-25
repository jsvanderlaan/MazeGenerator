using MazeGenerator.Cells;
using MazeGenerator.Common;
using MazeGenerator.Mazes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace MazeGenerator.Drawing
{
    public class MazeToImageConverter<C> where C : Cell, new()
    {
        private string _filePath;
        private Timer _timer;

        private readonly int _cellSize;
        private readonly double _cellWidth;
        private readonly double _cellHeight;

        private readonly Color _edgeColor;
        private readonly int _edgeWidth;
        private readonly Pen _edgePen;

        private readonly Brush _entranceBrush;
        private readonly Brush _exitBrush;
        private readonly Brush _inactiveBrush;

        private readonly Maze<C> _maze;

        private Graphics img;

        public MazeToImageConverter(Maze<C> maze)
        {
            var config = ConfigurationManager.GetSection("GraphicalMaze") as NameValueCollection;

            _cellSize = int.Parse(config["cellSize"]);
            _cellWidth = maze[0, 0].RelativeWidth() * _cellSize;
            _cellHeight = maze[0, 0].RelativeHeight() * _cellSize;

            _edgeColor = Color.FromName(config["edgeColor"]);
            _edgeWidth = int.Parse(config["edgeWidth"]);
            _edgePen = new Pen(_edgeColor, _edgeWidth);

            _entranceBrush = new SolidBrush(Color.FromName(config["entranceColor"]));
            _exitBrush = new SolidBrush(Color.FromName(config["exitColor"]));
            _inactiveBrush = new SolidBrush(Color.FromName(config["inactiveColor"]));

            _maze = maze;
        }

        public void SaveMaze(string filePath, bool drawPath)
        {
            _timer = new Timer("Saving maze");
            Bitmap bitmap = new Bitmap((int)Math.Round(_maze.Width * _cellWidth), (int)Math.Round(_maze.Height * _cellHeight));

            _timer.Start(_maze.Width * _maze.Height);
            using (img = Graphics.FromImage(bitmap))
            {
                if (drawPath) DrawPath();
                DrawExitCell(_maze.ExitCell);
                DrawEntranceCell(_maze.StartingCell);
                DrawInactiveCells();
                DrawWalls();
            }
            bitmap.Save(filePath, ImageFormat.Png);
            bitmap.Dispose();
            _timer.Stop();
        }

        public void DrawInactiveCells()
        {
            var cells = _maze.Cells().Where(c => !c.Active);
            foreach(var cell in cells)
            {
                DrawInactiveCell(cell);
                _timer.Next();
            }
        }

        public void DrawWalls()
        {
            var cells = _maze.Cells().Where(c => c.Active);
            foreach (var cell in cells)
            {
                DrawCellWalls(cell);
                _timer.Next();
            }
        }

        public void DrawPath()
        {
            C curr = _maze.ExitCell;
            Position end = _maze.StartingCell.Position;
            int dist = curr.Distance;
            while (curr.Position != end)
            {
                Brush brush = new SolidBrush(Extensions.GetColorInRange(curr.Distance, dist));
                DrawInnerCell(curr, brush);
                curr = _maze.Cell(curr.Predecessor);
            }
        }

        private void DrawCellWalls(C cell)
        {
            foreach(Wall wall in cell.Walls)
            {
                img.DrawLines(_edgePen, cell.GetWall(wall, _cellSize).ToArray());
            }
        }

        private void DrawInactiveCell(C cell)
        {
            DrawInnerCell(cell, _inactiveBrush);
        }

        private void DrawExitCell(C cell)
        {
            DrawInnerCell(cell, _exitBrush);
        }

        private void DrawEntranceCell(C cell)
        {
            DrawInnerCell(cell, _entranceBrush);
        }

        private void DrawInnerCell(C cell, Brush brush)
        {
            img.FillPolygon(brush, cell.GetCellCorners(_cellSize).ToArray());
        }
    }
}
