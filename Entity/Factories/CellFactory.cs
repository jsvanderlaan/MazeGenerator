using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Entities.Cells;

namespace Entities.Factories
{
    public class CellFactory : ICellFactory
    {
        public ICell CreateCell(Shape shape, int x, int y, bool active)
        {
            switch(shape) {
                case Shape.Hexagonal:
                    return new HexagonalCell(x, y, active);
                case Shape.Rectangular:
                    return new RectangularCell(x, y, active);
                case Shape.Triangular:
                    return new TriangularCell(x, y, active);
                default:
                    throw new ArgumentOutOfRangeException($"Shape {shape} not valid");
            }
        }
    }
}
