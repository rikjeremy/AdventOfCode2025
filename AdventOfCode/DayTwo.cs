using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal class DayTwo
    {
        List<(long, long)> Ranges = new();

        public DayTwo()
        {
            string input;

            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day2.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            input = sr.ReadToEnd().Trim();

            var RangesA = input.Split(",").ToList();

            foreach (var r in RangesA)
            {
                var values = r.Split("-");
                Ranges.Add((long.Parse(values[0]), long.Parse(values[1])));
            }
        }

        public long TotalInvalid(long min, long max)
        {
            var invalidTotal = 0L;

            for (var i = min; i <= max; i++)
            {
                var numberString = i.ToString();

                if (numberString.Length % 2 == 1)
                {
                    continue;
                }

                int halfLength = numberString.Length / 2;

                if (numberString[..halfLength].ToString() == numberString[halfLength..].ToString())
                {
                    Console.WriteLine($"{i} ({min}-{max})");
                    invalidTotal += i;
                }
            }

            return invalidTotal;
        }

        public long GetSumOfInvalid()
        {
            long total = 0;
            foreach (var r in Ranges)
            {
                total += TotalInvalid(r.Item1, r.Item2);
            }

            return total;
        }

        public long TotalInvalid2(long min, long max)
        {
            var invalidTotal = 0L;

            for (var i = min; i <= max; i++)
            {
                var numberString = i.ToString();

                if (numberString.Length == 1)
                {
                    continue;
                }

                if (numberString == new string(numberString[0], numberString.Length))
                {
                    Console.WriteLine($"{i} ({min}-{max})");
                    invalidTotal += i;
                    continue;
                }

                var alreadyAdded = false;
                for (var j = 2; j <= numberString.Length / 2; j++)
                {

                    if (numberString.Length % j != 0)
                    {
                        continue;
                    }

                    var repCharsStr = numberString.Substring(0, j);

                    var compStr = string.Concat(Enumerable.Repeat(repCharsStr, numberString.Length / j));

                    if (!alreadyAdded && compStr == numberString)
                    {
                        Console.WriteLine($"{i} ({min}-{max})");
                        invalidTotal += i;
                        alreadyAdded = true;
                    }
                }

            }

            return invalidTotal;
        }

        public long GetSumOfInvalid2()
        {
            long total = 0;
            foreach (var r in Ranges)
            {
                total += TotalInvalid2(r.Item1, r.Item2);
            }

            return total;
        }
    }
}
