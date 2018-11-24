using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;

namespace MazeGenerator
{
    public class GraphicalMaze : Maze
    {
        private string _filePath;
        private Timer _timer;

        private int _cellHeight;
        private int _cellWidth;

        private Color _edgeColor;
        private int _edgeWidth;
        private Pen _edgePen;

        private Brush _entranceBrush;
        private Brush _exitBrush;
        private Brush _inactiveBrush;

        private Graphics img;

        public GraphicalMaze(Boolean[,] grid) : base(grid)
        {
            Initialize();
        }

        public GraphicalMaze(int width, int height) : base(width, height)
        {
            Initialize();
        }

        public void Initialize()
        {
            var config = ConfigurationManager.GetSection("GraphicalMaze") as NameValueCollection;

            _cellHeight = int.Parse(config["cellHeight"]);
            _cellWidth = int.Parse(config["cellWidth"]);

            _edgeColor = Color.FromName(config["edgeColor"]);
            _edgeWidth = int.Parse(config["edgeWidth"]);
            _edgePen = new Pen(_edgeColor, _edgeWidth);

            _entranceBrush = new SolidBrush(Color.FromName(config["entranceColor"]));
            _exitBrush = new SolidBrush(Color.FromName(config["exitColor"]));
            _inactiveBrush = new SolidBrush(Color.FromName(config["inactiveColor"]));
        }

        public void DrawMaze(string filePath, bool drawPath)
        {
            _timer = new Timer("Drawing maze");
            Bitmap bitmap = new Bitmap(_width * _cellWidth, _height * _cellHeight);

            _timer.Start(_width * _height);
            using (img = Graphics.FromImage(bitmap))
            {
                if (drawPath) DrawPath();
                DrawExitCell(ExitCell);
                DrawEntranceCell(StartingPoint);
                DrawInactiveCells();
                DrawEdges();
            }
            bitmap.Save(filePath, ImageFormat.Png);
            bitmap.Dispose();
            _timer.Stop();
        }

        public void DrawInactiveCells()
        {
            var cells = GetCells(false, false);
            foreach(var cell in cells)
            {
                DrawInactiveCell(cell);
                _timer.Next();
            }
        }

        public void DrawEdges()
        {
            var cells = GetCells(true, false);
            foreach(var cell in cells)
            {
                DrawCellEdges(cell);
                _timer.Next();
            }
        }

        public void DrawPath()
        {
            Cell curr = ExitCell;
            int dist = curr.Distance;
            while (curr.Position != StartingPoint.Position)
            {
                Brush brush = new SolidBrush(Extensions.GetColorInRange(curr.Distance, dist));
                DrawInnerCell(curr, brush);
                curr = _cells[curr.Predecessor.X, curr.Predecessor.Y];
            }
        }

        public void DrawCell(Cell cell)
        {
            if (cell.State.HasFlag(CellState.Active))
            {
                DrawCellEdges(cell);
            }
            else
            {
                DrawInactiveCell(cell);
            }
        }

        private void DrawCellEdges(Cell cell)
        {
            Rectangle rect = GetCellRectangle(cell);

            if (cell.State.HasFlag(CellState.Top)) img.DrawLine(_edgePen, rect.TopLeft(), rect.TopRight());
            if (cell.State.HasFlag(CellState.Right)) img.DrawLine(_edgePen, rect.TopRight(), rect.BottomRight());
            if (cell.State.HasFlag(CellState.Bottom)) img.DrawLine(_edgePen, rect.BottomLeft(), rect.BottomRight());
            if (cell.State.HasFlag(CellState.Left)) img.DrawLine(_edgePen, rect.TopLeft(), rect.BottomLeft());
        }

        private void DrawInactiveCell(Cell cell)
        {
            DrawInnerCell(cell, _inactiveBrush);
        }

        private void DrawExitCell(Cell cell)
        {
            DrawInnerCell(cell, _exitBrush);
        }

        private void DrawEntranceCell(Cell cell)
        {
            DrawInnerCell(cell, _entranceBrush);
        }

        private void DrawInnerCell(Cell cell, Brush brush)
        {
            Rectangle rect = GetCellRectangle(cell);

            img.FillRectangle(brush, rect);
        }

        //private Rectangle GetInnerCellRectangle(Cell cell) {
        //    var correction = _edgeWidth / 2;
        //    return new Rectangle(
        //        (cell.Position.X * _cellWidth) + correction,
        //        (cell.Position.Y * _cellHeight) + correction,
        //        _cellWidth - (2 * correction),
        //        _cellHeight - (2 * correction));
        //}

        private Rectangle GetCellRectangle(Cell cell) => new Rectangle(
            (cell.Position.X * _cellWidth),
            (cell.Position.Y * _cellHeight),
            _cellWidth,
            _cellHeight);

        private Color GetCellColor(Cell cell)
        {
            if (!cell.State.HasFlag(CellState.Active)) return Color.Black;
            return Color.Red;
        }
    }
}
