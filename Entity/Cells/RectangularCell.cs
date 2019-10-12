using Common;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Entities.Cells
{
    public class RectangularCell : Cell
    {
        public RectangularCell(int x = -1, int y = - 1, bool active = false) : base(x, y, active)
        {
            Walls = new List<Wall>
            {
                Wall.Top,
                Wall.Left,
                Wall.Right,
                Wall.Bottom,
            };
            Shape = Shape.Rectangular;
        }

        public override ICollection<Position> GetNeighbours() => new List<Position>
        {
            new Position(Position.X - 1, Position.Y),
            new Position(Position.X, Position.Y - 1),
            new Position(Position.X + 1, Position.Y),
            new Position(Position.X, Position.Y + 1),
        };

        public override Wall GetWallForNeighbour(Position p)
        {
            if (Position.X - 1 == p.X && Position.Y == p.Y) return Wall.Left;
            if (Position.X + 1 == p.X && Position.Y == p.Y) return Wall.Right;
            if (Position.X == p.X && Position.Y - 1 == p.Y) return Wall.Top;
            if (Position.X == p.X && Position.Y + 1 == p.Y) return Wall.Bottom;
            throw new Exception($"{p} is not a neighbour of {this}");
        }

        public override double RelativeWidth() => 1.0;
        public override double RelativeHeight() => 1.0;
        private Point TopLeft(int cellSize) => new Point((int)(Position.X * RelativeWidth() * cellSize), (int)(Position.Y * RelativeHeight() * cellSize));

        public override IEnumerable<Point> GetWall(Wall wall, int cellSize)
        {
            var p = TopLeft(cellSize);
            yield return (wall == Wall.Bottom || wall == Wall.Right) ? new Point(p.X + cellSize, p.Y + cellSize) : new Point(p.X, p.Y);
            yield return (wall == Wall.Top || wall == Wall.Right) ? new Point(p.X + cellSize, p.Y) : new Point(p.X, p.Y + cellSize);
        }

        public override IEnumerable<Point> GetCellCorners(int cellSize)
        {
            var p = TopLeft(cellSize);

            yield return new Point(p.X, p.Y);
            yield return new Point(p.X + cellSize, p.Y);
            yield return new Point(p.X + cellSize, p.Y + cellSize);
            yield return new Point(p.X, p.Y + cellSize);
        }

        public override Point GetCellCenter(int cellSize) => new Point((int)(Position.X * RelativeWidth() * cellSize + 0.5 * cellSize), (int)(Position.Y * RelativeHeight() * cellSize + 0.5 * cellSize));
    }
}
