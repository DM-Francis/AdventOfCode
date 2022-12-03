using System;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            var waitingArea = new WaitingArea(input);

            int changes = 1;

            while (changes > 0)
            {
                changes = waitingArea.IncrementRoundV2();
            }

            int occupiedCount = 0;
            for (int row = 0; row < waitingArea.Seats.GetLength(0); row++)
            {
                for (int col = 0; col < waitingArea.Seats.GetLength(1); col++)
                {
                    if (waitingArea.Seats[row, col] == SeatState.Occupied)
                        occupiedCount++;
                }
            }

            Console.WriteLine(occupiedCount);
        }
    }
}
