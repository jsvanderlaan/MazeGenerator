using MazeGenerator.Common;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MazeGenerator.Cells
{
    public class TriangularCell : Cell
    {
        public readonly bool TopWall;

        public TriangularCell(int x = -1, int y = -1, bool active = false) : base(x, y, active)
        {
            Walls = new List<Wall>
            {
                ((x + y) % 2 == 0) ? Wall.Top : Wall.Bottom,
                Wall.Right,
                Wall.Left,
            };
            Shape = Shape.Hexagonal;
            TopWall = (Position.X + Position.Y) % 2 == 0;
        }

        public TriangularCell() : this(-1, -1)
        {

        }

        public override ICollection<Position> GetNeighbours() => new List<Position>
        {
            new Position(Position.X - 1, Position.Y),
            new Position(Position.X, Position.Y + (TopWall ? -1 : 1)),
            new Position(Position.X + 1, Position.Y),
        };

        public override Wall GetWallForNeighbour(Position p)
        {
            if (Position.X - 1 == p.X && Position.Y == p.Y) return Wall.Left;
            if (Position.X + 1 == p.X && Position.Y == p.Y) return Wall.Right;
            if (TopWall && Position.X == p.X && Position.Y - 1 == p.Y) return Wall.Top;
            if (!TopWall && Position.X == p.X && Position.Y + 1 == p.Y) return Wall.Bottom;
            throw new Exception($"{p} is not a neighbour of {this}");
        }

        public override double RelativeWidth() => 0.5;
        public override double RelativeHeight() => 1.0;
        private double RelativeX(int cellSize) => Position.X * RelativeWidth() * cellSize;
        private double RelativeY(int cellSize) => Position.Y * RelativeHeight() * cellSize;

        public override IEnumerable<Point> GetWall(Wall wall, int cellSize)
        {
            var x = RelativeX(cellSize);
            var y = RelativeY(cellSize);

            yield return wall == Wall.Right 
                ? new Point((int)(x + cellSize), (int)(y + (TopWall ? 0 : cellSize))) 
                : new Point((int)x, (int)(y + (TopWall ? 0.0 : cellSize)));
            yield return wall == Wall.Top || wall == Wall.Bottom 
                ? new Point((int)(x + cellSize), (int)(y + (TopWall ? 0 : cellSize)))
                : new Point((int)(x + cellSize / 2.0), (int)(y + (TopWall ? cellSize : 0)));
        }

        public override IEnumerable<Point> GetCellCorners(int cellSize)
        {
            var x = RelativeX(cellSize);
            var y = RelativeY(cellSize);

            yield return new Point((int)x, (int)(y + (TopWall ? 0 : cellSize)));
            yield return new Point((int)(x + cellSize), (int)(y + (TopWall ? 0.0 : cellSize)));
            yield return new Point((int)(x + cellSize / 2.0), (int)(y + (TopWall ? cellSize : 0.0)));
        }

        public override Point GetCellCenter(int cellSize) => new Point((int)(RelativeX(cellSize) + 0.5 * cellSize), (int)(RelativeY(cellSize) + 0.5 * cellSize));
    }
}
