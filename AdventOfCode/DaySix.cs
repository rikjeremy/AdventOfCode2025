using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal class DaySix
    {

        private List<Sum> sums = [];

        private void LoadDataV1()
        {
            sums = [];
            string input;

            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day6.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            bool numbers = true;
            bool create = true;

            while (!sr.EndOfStream)
            {
                input = sr.ReadLine()?.Trim() ?? string.Empty;

                List<string> row = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();


                if (!int.TryParse(row[0], out _))
                {
                    numbers = false;
                }

                if (create)
                {
                    sums.AddRange(row.Select(o => new Sum() { Values = [int.Parse(o)] }));
                }
                else
                {

                    for (int i = 0; i < row.Count; i++)
                        if (numbers)
                        {
                            sums[i].Values.Add(int.Parse(row[i]));
                        }
                        else
                        {
                            sums[i].Op = row[i].ToCharArray()[0];
                        }

                }

                create = false;
            }
        }

        private void LoadDataV2()
        {
            sums = [];
            string input;
            List<List<char>> rowChars = new();

            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day6.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                input = sr.ReadLine() ?? string.Empty;
                var row = input.Reverse().ToList();

                if (row.Count > 0)
                {
                    rowChars.Add(row);
                }
            }

            var transposed = rowChars
                .SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, v => v.item)
                .Select(g => g.ToList())
                .ToList();

            var currentSum = new Sum();

            foreach (var row in transposed)
            {

                if (row.TrueForAll(o => o == ' '))
                {
                    if (currentSum.Values.Count != 0)
                    {
                        sums.Add(currentSum);
                    }
                    currentSum = new Sum();
                    continue;
                }

                var rowValStr = new string(row.ToArray()).Trim();
                char? op = rowValStr[^1] switch
                {
                    '*' => '*',
                    '+' => '+',
                    _ => null
                };

                if (op is not null)
                {
                    currentSum.Op = (char)op;
                    rowValStr = rowValStr[..^1];
                }

                currentSum.Values.Add(int.Parse(rowValStr));
            }
            sums.Add(currentSum);
        }

        public long GetCheckTotal(int version)
        {
            switch (version)
            {
                case 1:
                    LoadDataV1();
                    break;
                case 2:
                    LoadDataV2();
                    break;
                default:
                    throw new NotImplementedException();
            }

            long total = 0;
            foreach (var sum in sums)
            {
                total += sum.Result;
            }
            return total;
        }

        private class Sum
        {
            public List<int> Values { get; set; } = [];

            public char Op { get; set; }

            public long Result
            {
                get
                {
                    long res = 0;

                    if (Op == '*')
                    {
                        foreach (var val in Values)
                        {
                            if (res == 0)
                            {
                                res = val;
                            }
                            else
                            {
                                res *= val;
                            }
                        }
                    }

                    if (Op == '+')
                    {
                        res = Values.Sum();
                    }

                    return res;
                }
            }
        }
    }
}
