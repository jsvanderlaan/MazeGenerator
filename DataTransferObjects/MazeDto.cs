using Common;

namespace DataTransferObjects
{
    public class MazeDto
    {
        public string Id { get; set; }
        public int Width { get; set; }
        public int Heigth { get; set; }
        public int NumberOfCells { get; set; }
        public int Length { get; set; }
        public GenerationType GenerationType { get; set; }
        public Shape Shape { get; set; }

        //Todo: processtimes
    }
}
