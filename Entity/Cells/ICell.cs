using System.Collections.Generic;
using System.Drawing;
using Common;
using Common.Enums;

namespace Entities.Cells
{
    public interface ICell
    {
        Position Position { get; }
        Position Predecessor { get; set; }
        bool Active { get; }
        bool Visited { get; set; }
        int Distance { get; set; }
        ICollection<Wall> Walls { get; set; }
        ICollection<Position> Successors { get; set; }

        bool HasWall(Wall top);
        ICollection<Position> GetNeighbours();
        void RemoveWall(Wall wall);
        Wall GetWallForNeighbour(Position cell);

        //Drawing
        Point GetCellCenter(int cellSize);
        IEnumerable<Point> GetCellCorners(int cellSize);
        IEnumerable<Point> GetWall(Wall wall, int cellSize);

        double RelativeHeight();
        double RelativeWidth();
    }
}
