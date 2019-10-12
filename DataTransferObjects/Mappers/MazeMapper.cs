using Common.Enums;
using Entities.Mazes;

namespace DataTransferObjects.Mappers
{
    public class MazeMapper
    {
        public MazeDto Map(Maze entity) => new MazeDto
        {
            Heigth = entity.Height,
            Width = entity.Width,
            Shape = entity.Shape,
            Length = entity.Length,
            GenerationType = GenerationType.RandomList,
            NumberOfCells = entity.NumberOfCells
        };
    }
}
