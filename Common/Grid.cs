namespace Common
{
    //Todo: implement
    public class Grid<T> : IGrid
    {
        private T[,] _grid;
        public Grid(T[,] grid)
        {
            _grid = grid;
        }

        public int Height => _grid.GetLength(1);
        public int Width => _grid.GetLength(0);
        public T GetPosition(int x, int y) => _grid[x, y];
        public void SetPosition(int x, int y, T value) => _grid[x, y] = value;

        public void FillGrid(T value)
        {
            for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) SetPosition(i, j, value);
        }
    }
}
