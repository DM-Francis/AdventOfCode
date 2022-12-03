using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    public class Board
    {
        private readonly int[,] _numbers;
        private readonly bool[,] _marked;

        public int[,] Numbers => (int[,])_numbers.Clone();
        public bool[,] Marked => (bool[,])_marked.Clone();

        public int Size { get; }

        public Board(IEnumerable<string> inputData)
        {
            var data = inputData.ToList();

            Size = data.Count;
            _numbers = new int[Size, Size];
            _marked = new bool[Size, Size];

            int currentLine = 0;
            foreach(var line in inputData)
            {
                var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();

                if (numbers.Count != Size)
                    throw new ArgumentException("Expecting square grid of numbers", nameof(inputData));

                for (int i = 0; i < Size; i++)
                {
                    _numbers[currentLine, i] = numbers[i];
                }

                currentLine++;
            }
        }

        public void Mark(int number)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (_numbers[x,y] == number)
                        _marked[x,y] = true;
                }
            }
        }

        public bool HasWon()
        {
            for (int i = 0; i < Size; i++)
            {
                if (RowHasWon(i))
                    return true;
                if (ColHasWon(i))
                    return true;
            }

            return false;
        }

        public int[]? GetWinningNumbers()
        {
            for (int i = 0; i < Size; i++)
            {
                if (RowHasWon(i))
                    return _numbers.GetRow(i);

                if (ColHasWon(i))
                    return _numbers.GetColumn(i);
            }

            return null;
        }

        public IEnumerable<int> GetAllUnmarkedNumbers()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (!_marked[row, col])
                        yield return _numbers[row, col];
                }
            }
        }

        private bool RowHasWon(int row)
        {
            for (int col = 0; col < Size; col++)
            {
                if (!_marked[row,col])
                    return false;
            }

            return true;
        }

        private bool ColHasWon(int col)
        {
            for (int row = 0; row < Size; row++)
            {
                if (!_marked[row, col])
                    return false;
            }

            return true;
        }

        public void WriteToConsole()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (_marked[row, col])
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ResetColor();

                    Console.Write($"{_numbers[row, col],2}");
                    if (col != Size - 1)
                        Console.Write(' ');

                }

                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
}
