using MazeGenerator.Common;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MazeGenerator.Cells
{
    public abstract class Cell
    {
        public Position Position;
        public Position Predecessor;
        public ICollection<Position> Successors;
        public ICollection<Wall> Walls;
        public bool Visited;
        public bool Active;
        public int Distance;
        public Shape Shape;

        public Cell(int x = -1, int y = -1, bool active = false)
        {
            Position = new Position(x, y);
            Active = active;
            Distance = -1;
            Visited = false;
            Predecessor = new Position(-1, -1);
            Successors = new List<Position>();
        }

        public bool HasWall(Wall wall) => Walls.Contains(wall);

        public void RemoveWall(Wall wall)
        {
            if (!Walls.Remove(wall))
            {
                throw new Exception("Wall does not exist.");
            }
        }


        public abstract ICollection<Position> GetNeighbours();

        public abstract Wall GetWallForNeighbour(Position cell);

        public override string ToString()
        {
            return Position.ToString();
        }

        public abstract double RelativeWidth();
        public abstract double RelativeHeight();

        public abstract Point GetCellCenter(int cellSize);
        public abstract IEnumerable<Point> GetCellCorners(int cellSize);
        public abstract IEnumerable<Point> GetWall(Wall wall, int cellSize);
    }
}
