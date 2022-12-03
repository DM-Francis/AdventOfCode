namespace Common
{
    public static class ArrayExtensions
    {
        public static T[] GetRow<T>(this T[,] array, int row)
        {
            return Enumerable.Range(0, array.GetLength(1))
                .Select(col => array[row, col])
                .ToArray();
        }

        public static T[] GetColumn<T>(this T[,] array, int col)
        {
            return Enumerable.Range(0, array.GetLength(0))
                .Select(row => array[row, col])
                .ToArray();
        }
    }
}