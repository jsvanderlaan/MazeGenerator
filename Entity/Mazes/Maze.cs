using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Common;
using Entities.Cells;
using System.Text;
using Entities.Factories;

namespace Entities.Mazes
{
    public class Maze
    {
        public int Width;
        public int Height;

        //public Shape Shape;

        private ICell[,] _cells;

        //Todo: can this be positions?
        private ICell _startingCell;
        private ICell _finalCell;
        private ICell _exitCell;
        private ICell _entranceCell;

        private Random _rng = new Random();
        private readonly bool _useEdges;
        private readonly ICellFactory _cellFactory;

        public Maze(Boolean[,] grid, Shape shape, bool useEdges)
        {
            //Shape = shape;
            Width = grid.GetLength(0) + 2;
            Height = grid.GetLength(1) + 2;

            _cellFactory = new CellFactory();

            var initTimer = new Timer("Initializing maze");
            initTimer.Start(Width * Height);

            _useEdges = useEdges;

            _cells = new ICell[Width, Height];

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var active = !(x == 0 || x == Width - 1 || y == 0 || y == Height - 1) && grid[x - 1, y - 1];
                    _cells[x, y] = _cellFactory.CreateCell(shape, x, y, active);
                    initTimer.Next();
                }
            }

            _startingCell = _cellFactory.CreateCell(shape, -1, -1, false);
            _exitCell = _cellFactory.CreateCell(shape, -1, -1, false);

            initTimer.Stop();
            //GenerateWithStack(StartingCell.Position);
        }

        public void ConsoleDisplay()
        {
            var firstLine = string.Empty;
            for (var y = 0; y < Height; y++)
            {
                var sbTop = new StringBuilder();
                var sbMid = new StringBuilder();
                for (var x = 0; x < Width; x++)
                {
                    sbTop.Append(this[x, y].HasWall(Wall.Top) ? "+-" : "+ ");
                    sbMid.Append(this[x, y].HasWall(Wall.Left) ? "| " : "  ");
                }
                if (firstLine == string.Empty)
                    firstLine = sbTop.ToString();
                Console.WriteLine(sbTop + "+");
                Console.WriteLine(sbMid + "|");
            }
            Console.WriteLine(firstLine);
        }

        private static Boolean[,] GetGrid(int width, int height)
        {
            var grid = new Boolean[width, height];
            for (int i = 0; i < width; i++) for (int j = 0; j < height; j++) grid[i, j] = true;
            return grid;
        }

        public ICell this[int x, int y] => _cells[x, y];
        public IEnumerable<ICell> Cells() => _cells.Cast<ICell>();
        public ICell Cell(Position p) => _cells[p.X, p.Y];
        public IEnumerable<ICell> Cells(IEnumerable<Position> ps) => ps.Select(p => Cell(p));
        private IEnumerable<ICell> GetNeighbours(ICell cell) => cell.GetNeighbours().Where(p => p.X >= 0 && p.Y >= 0 && p.X < Width && p.Y < Height).Select(p => Cell(p));
        private bool IsEdgeCell(ICell cell) => GetNeighbours(cell).Any(c => !c.Active);

        public void GenerateWithRandomList(Position pos)
        {
            int size = Cells().Where(c => c.Active).Count();
            var stackTimer = new Timer("Creating maze with random list");
            stackTimer.Start(size);

            Position cPos = pos;
            int cDist = 0;

            List<Position> CellList = new List<Position>();
            do
            {
                var cell = Cell(cPos);
                cell.Visited = true;
                cell.Distance = cDist;

                var neighbours = GetNeighbours(cell).Where(c => c.Active && !c.Visited).Shuffle(_rng);

                if (neighbours.Count() > 0)
                {
                    var neighbour = neighbours.First();
                    CellList.Add(cPos);
                    cDist++;

                    RemoveWalls(cell, neighbour);
                    cell.Successors.Add(neighbour.Position);
                    neighbour.Predecessor = cPos;

                    cPos = neighbour.Position;

                    stackTimer.Next();
                }
                else
                {
                    CellList = CellList.Where(p => GetNeighbours(Cell(p)).Where(c => c.Active && !c.Visited).Count() > 0).Shuffle(_rng).ToList();
                    if (CellList.Count() > 0)
                    {
                        cPos = CellList.First();
                        cDist = Cell(cPos).Distance;
                    }
                }
            } while (CellList.Count > 0);

            FinishMaze();
            stackTimer.Stop();
        }

        public void GenerateWithStack(Position pos)
        {
            int size = Cells().Where(c => c.Active).Count();
            var stackTimer = new Timer("Creating maze with stack");
            stackTimer.Start(size);

            Position cPos = pos;
            int cDist = 0;

            Stack<Position> CellStack = new Stack<Position>();
            do
            {
                var cell = Cell(cPos);
                cell.Visited = true;
                cell.Distance = cDist;

                var neighbours = GetNeighbours(cell).Where(c => c.Active && !c.Visited).Shuffle(_rng);

                if (neighbours.Count() > 0)
                {
                    var neighbour = neighbours.First();
                    CellStack.Push(cPos);
                    cDist++;

                    RemoveWalls(cell, neighbour);
                    cell.Successors.Add(neighbour.Position);
                    neighbour.Predecessor = cPos;

                    cPos = neighbour.Position;

                    stackTimer.Next();
                }
                else
                {
                    cPos = CellStack.Pop();
                    cDist = Cell(cPos).Distance;
                }
            } while (CellStack.Count > 0);

            FinishMaze();
            stackTimer.Stop();
        }

        private void FinishMaze()
        {
            //Create ExitCell
            var end = ExitCell;
        }

        private void RemoveWalls(ICell c1, ICell c2)
        {
            c1.RemoveWall(c1.GetWallForNeighbour(c2.Position));
            c2.RemoveWall(c2.GetWallForNeighbour(c1.Position));
        }

        public ICell EntranceCell
        {
            get
            {
                return _entranceCell;
            }
        }

        public ICell FinalCell
        {
            get
            {
                return _finalCell;
            }
        }

        public ICell StartingCell
        {
            get
            {
                if (_startingCell.Position != new Position(-1, -1))
                {
                    return _startingCell;
                }
                _startingCell = Cells().Where(c => c.Active && (!_useEdges || IsEdgeCell(c))).Random(_rng);
                _entranceCell = Cells(_startingCell.GetNeighbours()).Where(c => !c.Active).Random(_rng);
                RemoveWalls(_startingCell, _entranceCell);
                return _startingCell;
            }
        }

        public ICell ExitCell
        {
            get
            {
                if (_exitCell.Position != new Position(-1, -1))
                {
                    return _exitCell;
                }
                ICell curr = _startingCell;
                int dist = 0;

                foreach (ICell cell in _cells)
                {
                    if ((!_useEdges || IsEdgeCell(cell)) && cell.Distance > dist)
                    {
                        curr = cell;
                        dist = cell.Distance;
                    }
                }

                _exitCell = curr;
                _finalCell = Cells(_exitCell.GetNeighbours()).Where(c => !c.Active).Random(_rng);

                RemoveWalls(_exitCell, FinalCell);

                Debug.WriteLine($"The exit is at distance {dist} and position {curr.Position}");

                return curr;
            }
        }
    }
}
