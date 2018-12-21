using MazeGenerator.Common;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MazeGenerator.Cells
{
    public class HexagonalCell : Cell
    {
        public readonly bool Even;

        public HexagonalCell(int x = -1, int y = -1, bool active = false) : base(x, y, active)
        {
            Walls = new List<Wall>
            {
                Wall.Top,
                Wall.TopRight,
                Wall.Right,
                Wall.Bottom,
                Wall.BottomLeft,
                Wall.Left,
            };
            Shape = Shape.Hexagonal;
            Even = Position.Y % 2 == 0;
        }

        public HexagonalCell() : this(-1, -1)
        {

        }

        public override ICollection<Position> GetNeighbours() => new List<Position>
        {
            new Position(Position.X - 1, Position.Y),
            new Position(Position.X, Position.Y - 1),
            new Position(Position.X + (Even ? -1 : 1), Position.Y - 1),
            new Position(Position.X + 1, Position.Y),
            new Position(Position.X + (Even ? -1 : 1), Position.Y + 1),
        };

        public override Wall GetWallForNeighbour(Position p)
        {
            if (Position.X - 1 == p.X && Position.Y == p.Y) return Wall.Left;
            if (Position.X + 1 == p.X && Position.Y == p.Y) return Wall.Right;
            if (Even)
            {
                if (Position.X - 1 == p.X && Position.Y - 1 == p.Y) return Wall.BottomLeft;
                if (Position.X == p.X && Position.Y - 1 == p.Y) return Wall.Bottom;
                if (Position.X - 1 == p.X && Position.Y + 1 == p.Y) return Wall.Top;
                if (Position.X == p.X && Position.Y + 1 == p.Y) return Wall.TopRight;
            }
            else
            {
                if (Position.X + 1 == p.X && Position.Y - 1 == p.Y) return Wall.Bottom;
                if (Position.X == p.X && Position.Y - 1 == p.Y) return Wall.BottomLeft;
                if (Position.X + 1 == p.X && Position.Y + 1 == p.Y) return Wall.TopRight;
                if (Position.X == p.X && Position.Y + 1 == p.Y) return Wall.Top;
            }
            throw new Exception($"{p} is not a neighbour of {this}");
        }

        public override double RelativeWidth() => 1.0;
        public override double RelativeHeight() => Math.Sqrt(3) / 2.0;
        private double CenterX(int cellSize) => (Position.X + (Even ? 0.0 : 0.5)) * RelativeWidth() * cellSize;
        private double CenterY(int cellSize) => (Position.Y + 1.0 / 3.0) * RelativeHeight() * cellSize;
        private double Radius(int cellSize) => cellSize / Math.Sqrt(3);

        public override IEnumerable<Point> GetWall(Wall wall, int cellSize)
        {
            var x = CenterX(cellSize);
            var y = CenterY(cellSize);
            var r = Radius(cellSize);

            yield return GetCorner(wall, x, y, r);
            yield return GetCorner((Wall)((int)(wall + 1) % 6), x, y, r);
        }

        public override IEnumerable<Point> GetCellCorners(int cellSize)
        {
            var x = CenterX(cellSize);
            var y = CenterY(cellSize);
            var r = Radius(cellSize);

            for (int i = 0; i < 6; i++)
            {
                yield return GetCorner((Wall)i, x, y, r);
            }
        }

        private Point GetCorner(Wall wall, double x, double y, double r)
        {
            return new Point(
                (int)(x + r * Math.Cos((-1 * (int)wall * 60.0 + 150.0) * Math.PI / 180.0)),
                (int)(y + r * Math.Sin((-1 * (int)wall * 60.0 + 150.0) * Math.PI / 180.0)));
        }

        public override Point GetCellCenter(int cellSize) => new Point((int)CenterX(cellSize), (int)CenterY(cellSize));
    }
}
