using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    public class WaitingArea
    {
        private SeatState[,] _seats;
        private readonly int _rowCount;
        private readonly int _colCount;

        public SeatState[,] Seats => (SeatState[,])_seats.Clone();

        public WaitingArea(string[] inputData)
        {
            int rowCount = inputData.Length;
            int colCount = inputData[0].Length;

            _seats = new SeatState[rowCount, colCount];
            _rowCount = rowCount;
            _colCount = colCount;

            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    _seats[r, c] = inputData[r][c] switch
                    {
                        '.' => SeatState.Floor,
                        'L' => SeatState.Empty,
                        '#' => SeatState.Occupied,
                        _ => throw new ArgumentException($"Unrecognised seat character '{inputData[r][c]}'", nameof(inputData))
                    };
                }
            }
        }

        public int IncrementRound()
        {
            var newSeats = Seats;

            int changes = 0;
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    SeatState state = _seats[row, col];
                    if (state == SeatState.Floor)
                        continue;

                    int occupiedNearby = GetNearbySeats(row, col).Count(s => s == SeatState.Occupied);

                    if (state == SeatState.Empty && occupiedNearby == 0)
                    {
                        newSeats[row, col] = SeatState.Occupied;
                        changes++;
                    }
                    else if (state == SeatState.Occupied && occupiedNearby >= 4)
                    {
                        newSeats[row, col] = SeatState.Empty;
                        changes++;
                    }
                }
            }

            _seats = newSeats;

            return changes;
        }

        public int IncrementRoundV2()
        {
            var newSeats = Seats;

            int changes = 0;
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    SeatState state = _seats[row, col];
                    if (state == SeatState.Floor)
                        continue;

                    int occupiedNearby = GetVisibleSeats(row, col).Count(s => s == SeatState.Occupied);

                    if (state == SeatState.Empty && occupiedNearby == 0)
                    {
                        newSeats[row, col] = SeatState.Occupied;
                        changes++;
                    }
                    else if (state == SeatState.Occupied && occupiedNearby >= 5)
                    {
                        newSeats[row, col] = SeatState.Empty;
                        changes++;
                    }
                }
            }

            _seats = newSeats;

            return changes;
        }

        private List<SeatState> GetNearbySeats(int row, int col)
        {
            var nearbySeats = new List<SeatState>();

            if (row > 0)
                nearbySeats.Add(_seats[row - 1, col]);
            if (col > 0)
                nearbySeats.Add(_seats[row, col - 1]);
            if (row < _rowCount - 1)
                nearbySeats.Add(_seats[row + 1, col]);
            if (col < _colCount - 1)
                nearbySeats.Add(_seats[row, col + 1]);
            if (row > 0 && col > 0)
                nearbySeats.Add(_seats[row - 1, col - 1]);
            if (row > 0 && col < _colCount - 1)
                nearbySeats.Add(_seats[row - 1, col + 1]);
            if (row < _rowCount - 1 && col > 0)
                nearbySeats.Add(_seats[row + 1, col - 1]);
            if (row < _rowCount - 1 && col < _colCount - 1)
                nearbySeats.Add(_seats[row + 1, col + 1]);

            return nearbySeats;
        }

        private IEnumerable<SeatState> GetVisibleSeats(int row, int col)
        {
            var visibleSeats = new List<SeatState?>();

            // North
            visibleSeats.Add(GetVisibleSeatInDirection(row, col, 1, 0));

            // North East
            visibleSeats.Add(GetVisibleSeatInDirection(row, col, 1, 1));

            // East
            visibleSeats.Add(GetVisibleSeatInDirection(row, col, 0, 1));

            // South East
            visibleSeats.Add(GetVisibleSeatInDirection(row, col, -1, 1));

            // South
            visibleSeats.Add(GetVisibleSeatInDirection(row, col, -1, 0));

            // South West
            visibleSeats.Add(GetVisibleSeatInDirection(row, col, -1, -1));

            // West
            visibleSeats.Add(GetVisibleSeatInDirection(row, col, 0, -1));

            // North West
            visibleSeats.Add(GetVisibleSeatInDirection(row, col, 1, -1));

            return visibleSeats.Where(s => s.HasValue).Select(s => s.Value);
        }

        private SeatState? GetVisibleSeatInDirection(int baseRow, int baseCol, int northAmount, int eastAmount)
        {
            int rowMovement = eastAmount;
            int colMovement = -northAmount;

            int checkingRow = baseRow + rowMovement;
            int checkingCol = baseCol + colMovement;
            while (IndexesAreValid(checkingRow, checkingCol))
            {
                if (_seats[checkingRow, checkingCol] != SeatState.Floor)
                {
                    return _seats[checkingRow, checkingCol];
                }

                checkingRow += rowMovement;
                checkingCol += colMovement;
            }

            return null;
        }

        private bool IndexesAreValid(int row, int col)
            => row > 0 && col > 0 && row < _rowCount - 1 && col < _colCount - 1;
    }
}
