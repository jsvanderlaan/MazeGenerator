using Common;
using Common.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace Entities.Cells
{
    public abstract class Cell : ICell
    {
        public Position Position { get; }
        public Position Predecessor { get; set; }
        public ICollection<Position> Successors { get; set; }
        public ICollection<Wall> Walls { get; set; }
        public bool Visited { get; set; }
        public bool Active { get; }
        public int Distance { get; set; }
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

        public void RemoveWall(Wall wall) => Walls.Remove(wall);

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
