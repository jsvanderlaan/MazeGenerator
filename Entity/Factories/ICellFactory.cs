using Common;
using Entities.Cells;

namespace Entities.Factories
{
    public interface ICellFactory
    {
        ICell CreateCell(Shape shape, int x, int y, bool active);
    }
}