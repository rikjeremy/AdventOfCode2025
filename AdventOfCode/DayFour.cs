using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal class DayFour
    {
        private List<List<char>> plan = [];
        private List<List<char>> updatedPlan = [];

        public DayFour()
        {
            string input;

            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day4.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            var rowIndex = 0;
            while (!sr.EndOfStream)
            {
                input = sr.ReadLine()?.Trim() ?? string.Empty;
                plan.Add(input.ToCharArray().ToList());
                rowIndex++;
            }
        }

        public int CountAccessible()
        {
            var count = 0;

            for (int i = 0; i < plan.Count; i++)
            {
                count += CountAccessibleForRow(i);
            }

            return count;
        }

        public int CountAccessibleForRow(int row)
        {
            var rowCount = 0;

            for (int i = 0; i < plan[row].Count; i++)
            {
                if (plan[row][i] != '@')
                {
                    continue;
                }

                if (SurroundingCount(row, i) < 4)
                {
                    rowCount++;
                }
            }
            return rowCount;
        }

        public int CountAccessible2()
        {
            var count = 0;
            var removedCount = 0;
            do
            {
                removedCount = 0;
                for (int i = 0; i < plan.Count; i++)
                {
                    removedCount += CountAccessibleForRow2(i);
                }
                count += removedCount;
                plan = updatedPlan;
                updatedPlan = [];
            } while (removedCount != 0);
            return count;
        }

        public int CountAccessibleForRow2(int row)
        {

            var rowCount = 0;
            List<int> remove = [];

            for (int i = 0; i < plan[row].Count; i++)
            {
                if (plan[row][i] != '@')
                {
                    continue;
                }

                if (SurroundingCount(row, i) < 4)
                {
                    rowCount++;
                    remove.Add(i);
                }
            }

            updatedPlan.Add(plan[row]);
            foreach (int i in remove)
            {
                updatedPlan[row][i] = '.';
            }
            return rowCount;
        }

        public int SurroundingCount(int row, int col)
        {
            var count = 0;

            for (int xOffset = -1; xOffset < 2; xOffset++)
            {
                for (int yOffset = -1; yOffset < 2; yOffset++)
                {
                    var (x, y) = (col + xOffset, row + yOffset);

                    if (x >= 0 && x < plan[row].Count && y >= 0 && y < plan.Count && !(xOffset == 0 && yOffset == 0) && plan[y][x] == '@')
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
