using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal class DayThree
    {
        private List<string> Banks = [];
        private List<(long, string)> log = [];

        public DayThree()
        {
            string input;

            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day3.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                input = sr.ReadLine()?.Trim() ?? string.Empty;
                Banks.Add(input);
            }
        }

        public int GetBankMax2(string input)
        {
            List<int> bankValues = input.ToArray().Select(v => int.Parse(v.ToString())).ToList();

            // Get first Digit
            var firstDigit = bankValues[..^1].Max();
            var firstIndex = bankValues.IndexOf(firstDigit);
            var secondDigit = bankValues[(firstIndex + 1)..].Max();

            var value = (firstDigit * 10) + secondDigit;

            log.Add((value, input));

            return value;
        }

        public long GetTotal()
        {
            long total = 0;
            foreach (var bank in Banks)
            {
                total += GetBankMax2(bank);
            }
            return total;
        }

        public long GetBankMax12(string input)
        {

            List<int> bankValues = input.ToArray().Select(v => int.Parse(v.ToString())).ToList();
            List<int> digits = [];

            int startIndex = 0;
            long value = 0;

            for (int i = 0; i < 12; i++)
            {
                var digit = bankValues[startIndex..^(11 - i)].Max();
                startIndex += bankValues[startIndex..^(11 - i)].IndexOf(digit) + 1;
                value += digit * (long)Math.Pow(10, 11 - i);
            }

            log.Add((value, input));

            return value;
        }

        public long GetTotal2()
        {
            long total = 0;
            foreach (var bank in Banks)
            {
                total += GetBankMax12(bank);
            }
            return total;
        }
    }
}
