using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MazeGenerator
{
    public class Maze
    {
        private bool _showSteps;
        private bool _stopAtSteps;
        private bool _useEdges;

        protected Boolean[,] _grid;
        protected Cell[,] _cells;
        protected int _width;
        protected int _height;
        protected Point _startingPoint;

        private Random _rng = new Random();

        public Maze(Boolean[,] grid)
        {
            _grid = grid;
            _width = _grid.GetLength(0) + 2;
            _height = _grid.GetLength(1) + 2;
            _cells = new Cell[_width, _height];

            Init();
        }

        public Maze(int width, int height)
        {
            _grid = new Boolean[width, height];
            for (int i = 0; i < width; i++) for (int j = 0; j < height; j++) _grid[i, j] = true;
            _width = width + 2;
            _height = height + 2;
            _cells = new Cell[_width, _height];

            Init();
        }

        public void Init()
        {
            var config = ConfigurationManager.GetSection("Maze") as NameValueCollection;
            _showSteps = Boolean.Parse(config["showSteps"]);
            _stopAtSteps = Boolean.Parse(config["stopAtSteps"]);
            _useEdges = Boolean.Parse(config["useEdges"]);

            var initTimer = new Timer("Initializing maze");
            initTimer.Start(_width * _height);

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    initTimer.Next();
                    _cells[x, y].State = CellState.Initial;
                    _cells[x, y].Position = new Point(x, y);
                    _cells[x, y].Successors = new List<Point>();
                    if (!(x == 0 || x == _width - 1 || y == 0 || y == _height - 1) && _grid[x - 1, y - 1])
                    {
                        _cells[x, y].State |= CellState.Active;
                    }
                }
            }

            _startingPoint = GetRandomPoint();

            initTimer.Stop();

            GenerateWithStack(_startingPoint);
        }

        public void ConsoleDisplay()
        {
            var firstLine = string.Empty;
            for (var y = 0; y < _height; y++)
            {
                var sbTop = new StringBuilder();
                var sbMid = new StringBuilder();
                for (var x = 0; x < _width; x++)
                {
                    sbTop.Append(_cells[x, y].State.HasFlag(CellState.Top) ? "+-" : "+ ");
                    sbMid.Append(_cells[x, y].State.HasFlag(CellState.Left) ? "| " : "  ");
                }
                if (firstLine == string.Empty)
                    firstLine = sbTop.ToString();
                Console.WriteLine(sbTop + "+");
                Console.WriteLine(sbMid + "|");
            }
            Console.WriteLine(firstLine);
        }

        private Point GetRandomPoint()
        {
            var cells = GetCells(true, _useEdges);
            return cells.ElementAt(_rng.Next(cells.Count())).Position;
        }

        protected IEnumerable<Cell> GetCells(bool active, bool edge) => _cells.Cast<Cell>().Where(cell => active == cell.State.HasFlag(CellState.Active) && (!edge || IsEdgeCell(cell)));

        private bool IsEdgeCell(Cell cell) => GetAdjacentCells(cell.Position).Select(z => _cells[z.Position.X, z.Position.Y]).Any(c => !c.State.HasFlag(CellState.Active));

        private void GenerateWithStack(Point pos)
        {
            Point cPos = pos;
            int cDist = 0;

            int size = GetMazeSize();
            var stackTimer = new Timer("Creating maze");
            stackTimer.Start(size);

            Stack<Point> CellStack = new Stack<Point>();
            do
            {
                if (_showSteps)
                {
                    Console.Clear();
                    ConsoleDisplay();
                    if (_stopAtSteps) Console.ReadKey();
                }

                var adjacentCells = GetAdjacentCells(_cells[cPos.X, cPos.Y].Position).Shuffle(_rng).Where(z => _cells[z.Position.X, z.Position.Y].State.HasFlag(CellState.Active) && !_cells[z.Position.X, z.Position.Y].State.HasFlag(CellState.Visited));

                _cells[cPos.X, cPos.Y].State |= CellState.Visited;
                _cells[cPos.X, cPos.Y].Distance = cDist;

                if (adjacentCells.Count() > 0)
                {
                    stackTimer.Next();
                    var adjacentCell = adjacentCells.First();
                    CellStack.Push(cPos);
                    cDist++;

                    _cells[cPos.X, cPos.Y].State -= adjacentCell.Wall;
                    _cells[cPos.X, cPos.Y].Successors.Add(adjacentCell.Position);
                    _cells[adjacentCell.Position.X, adjacentCell.Position.Y].State -= adjacentCell.Wall.OppositeWall();
                    _cells[adjacentCell.Position.X, adjacentCell.Position.Y].Predecessor = cPos;

                    cPos = adjacentCell.Position;
                }
                else
                {
                    cPos = CellStack.Pop();
                    cDist = _cells[cPos.X, cPos.Y].Distance;
                }
            } while (CellStack.Count > 0);

            stackTimer.Stop();
        }

        public Cell ExitCell
        {
            get {
                Cell curr = StartingPoint;
                int dist = 0;

                foreach (Cell cell in _cells)
                {
                    if ((!_useEdges || IsEdgeCell(cell)) && cell.Distance > dist)
                    {
                        curr = cell;
                        dist = cell.Distance;
                    }
                }

                Debug.WriteLine($"The exit is at distance {dist} and position {curr.Position}");

                return curr;
            }
        }

        public Cell StartingPoint
        {
            get => _cells[_startingPoint.X, _startingPoint.Y];
        }

        public int GetMazeSize()
        {
            int count = 0;

            foreach (Cell cell in _cells)
            {
                if(cell.State.HasFlag(CellState.Active)) count++;
            }

            return count;
        }

        private IEnumerable<AdjacentCell> GetAdjacentCells(Point p)
        {
            if (p.X > 0) yield return new AdjacentCell { Position = new Point(p.X - 1, p.Y), Wall = CellState.Left };
            if (p.Y > 0) yield return new AdjacentCell { Position = new Point(p.X, p.Y - 1), Wall = CellState.Top };
            if (p.X < _width - 1) yield return new AdjacentCell { Position = new Point(p.X + 1, p.Y), Wall = CellState.Right };
            if (p.Y < _height - 1) yield return new AdjacentCell { Position = new Point(p.X, p.Y + 1), Wall = CellState.Bottom };
        }

        private void VisitCell(Point pos)
        {
            _cells[pos.X, pos.Y].State |= CellState.Visited;
            foreach (var p in GetAdjacentCells(pos).Shuffle(_rng).Where(z => !(_cells[z.Position.X, z.Position.Y].State.HasFlag(CellState.Visited))))
            {
                _cells[pos.X, pos.Y].State -= p.Wall;
                _cells[p.Position.X, p.Position.Y].State -= p.Wall.OppositeWall();
                VisitCell(p.Position);
            }
        }
    }
}
