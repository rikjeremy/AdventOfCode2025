using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    internal class DayOne
    {

        private const int dialStart = 50;
        private const int dialMin = 0;
        private const int dialMax = 99;

        private readonly List<string> input = [];


        public DayOne()
        {
            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day1.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line != null)
                {
                    input.Add(line);
                }
            }
        }

        public int GetPassword()
        {
            int output = 0;
            int rot = 0;
            int current = dialStart;

            foreach (string rotation in input)
            {
                rot++;
                var rotVal = ParseInstruction(rotation);
                var val = rotVal % 100;

                var intermediate = current + val;
                var prior = current;

                current = intermediate switch
                {
                    < dialMin => (dialMax + 1) + intermediate,
                    > dialMax => (dialMin - 1) + (intermediate - dialMax),
                    _ => intermediate
                };


                Console.WriteLine($"{rot:0000} - {rotation} - {prior} - {current}");

                if (current == 0 && val != 0)
                {
                    output++;
                }

            }

            return output;
        }

        public int GetPassword2()
        {
            int output = 0;
            int rot = 0;
            int current = dialStart;

            foreach (string rotation in input)
            {
                rot++;
                var rotVal = ParseInstruction(rotation);
                var val = rotVal % 100;
                var addZero = (int)Math.Floor((decimal)Math.Abs(rotVal) / 100);

                var intermediate = current + val;
                var prior = current;

                current = intermediate switch
                {
                    < dialMin => (dialMax + 1) + intermediate,
                    > dialMax => (dialMin - 1) + (intermediate - dialMax),
                    _ => intermediate
                };

                if (prior != 0 && current != 0 && ((val > 0 && current < prior) || (val < 0 && current > prior)))
                {
                    output++;
                }

                Console.WriteLine($"{rot:0000} - {rotation} - {prior} - {current}");

                output += addZero;

                if (current == 0 && val != 0)
                {
                    output++;
                }

            }
            return output;
        }

        private int ParseInstruction(string instruction)
        {
            int directionMultiple = char.ToUpper(instruction[0]) switch
            {
                'L' => -1,
                'R' => 1,
                _ => 0
            };

            int value = int.Parse(instruction[1..].ToString());

            return value * directionMultiple;
        }

    }
}
