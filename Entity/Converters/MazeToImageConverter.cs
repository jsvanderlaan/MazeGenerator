using Entities.Mazes;
using Common;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Entities.Cells;
using System.IO;

namespace Entities.Converters
{
    public class MazeToImageConverter
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

        private readonly Maze _maze;

        private Graphics img;

        public MazeToImageConverter(Maze maze)
        {
            _cellSize = 20; // int.Parse(config["cellSize"]);
            _cellWidth = maze[0, 0].RelativeWidth() * _cellSize;
            _cellHeight = maze[0, 0].RelativeHeight() * _cellSize;

            _edgeColor = Color.Black; // Color.FromName(config["edgeColor"]);
            _edgeWidth = 1; // int.Parse(config["edgeWidth"]);
            _edgePen = new Pen(_edgeColor, _edgeWidth);

            _entranceBrush = new SolidBrush(Color.Red); // new SolidBrush(Color.FromName(config["entranceColor"]));
            _exitBrush = new SolidBrush(Color.Red); //new SolidBrush(Color.FromName(config["exitColor"]));
            _inactiveBrush = new SolidBrush(Color.Transparent); //new SolidBrush(Color.FromName(config["inactiveColor"]));

            _maze = maze;
        }

        public byte[] GetMaze(bool drawPath, string path)
        {
            _timer = new Timer("Saving maze");
            using (Bitmap bitmap = new Bitmap((int)Math.Round(_maze.Width * _cellWidth), (int)Math.Round(_maze.Height * _cellHeight)))
            using (var stream = new MemoryStream())
            using (_entranceBrush)
            using (_exitBrush)
            using (_inactiveBrush)
            using(_edgePen)
            {

                _timer.Start(_maze.Width * _maze.Height);
                using (img = Graphics.FromImage(bitmap))
                {
                    if (drawPath) DrawPath();
                    //DrawExitCell(_maze.ExitCell);
                    //DrawEntranceCell(_maze.StartingCell);
                    DrawInactiveCells();
                    DrawWalls();
                    DrawStart();
                    DrawEnd();
                };
                var postfix = drawPath ? "solution" : "maze";
                bitmap.Save(path, ImageFormat.Png);
                bitmap.Save(stream, ImageFormat.Png);
                _timer.Stop();
                return stream.ToArray();
            }
        }

        public void DrawStart()
        {
            var centerFrom = _maze.EntranceCell.GetCellCenter(_cellSize);
            var centerTo = _maze.StartingCell.GetCellCenter(_cellSize);
            DrawTriangle(centerFrom, centerTo, 0.25 * _cellSize);
        }

        public void DrawEnd()
        {
            var centerFrom = _maze.FinalCell.GetCellCenter(_cellSize);
            var centerTo = _maze.ExitCell.GetCellCenter(_cellSize);
            DrawTriangle(centerFrom, centerTo, 0.25 * _cellSize);
        }

        private void DrawTriangle(Point from, Point to, double percentage)
        {
            var vec = from.GetVector(to, percentage);
            var left = new PointF(-1 * vec.Y, vec.X);
            var right = new PointF(vec.Y, -1 * vec.X);
            var points = new Point[] { from.Move(left), from.Move(vec), from.Move(right) };
            img.FillPolygon(_entranceBrush, points);
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
            ICell curr = _maze.ExitCell;
            Position end = _maze.StartingCell.Position;
            int dist = curr.Distance;
            Brush brush = new SolidBrush(Extensions.GetColorInRange(curr.Distance, dist));
            DrawInnerCell(curr, brush);
            while (curr.Position != end)
            {
                curr = _maze.Cell(curr.Predecessor);
                brush = new SolidBrush(Extensions.GetColorInRange(curr.Distance, dist));
                DrawInnerCell(curr, brush);
            }
        }

        private void DrawCellWalls(ICell cell)
        {
            foreach(Wall wall in cell.Walls)
            {
                img.DrawLines(_edgePen, cell.GetWall(wall, _cellSize).ToArray());
            }
        }

        private void DrawInactiveCell(ICell cell)
        {
            DrawInnerCell(cell, _inactiveBrush);
        }

        private void DrawExitCell(ICell cell)
        {
            DrawInnerCell(cell, _exitBrush);
        }

        private void DrawEntranceCell(ICell cell)
        {
            DrawInnerCell(cell, _entranceBrush);
        }

        private void DrawInnerCell(ICell cell, Brush brush)
        {
            img.FillPolygon(brush, cell.GetCellCorners(_cellSize).ToArray());
        }
    }
}
