using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal class DayFive
    {
        private readonly List<Range> RawRanges = [];
        private List<Range> ConsolidatedRanges = [];
        private readonly List<long> Items = [];

        public DayFive()
        {

            string input;

            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day5.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            var ranges = true;
            while (!sr.EndOfStream)
            {
                input = sr.ReadLine()?.Trim() ?? string.Empty;
                if (input == string.Empty)
                {
                    ranges = false;
                    continue;
                }
                if (ranges)
                {
                    var values = input.Split('-');
                    RawRanges.Add(new Range() { Start = long.Parse(values[0]), End = long.Parse(values[1]) });
                }
                else
                {
                    Items.Add(long.Parse(input));
                }
            }

            ConsolidateRanges();
        }

        public void ConsolidateRanges()
        {
            LinkedList<Range> tempRanges = new(RawRanges.OrderBy(o => o.Start));

            LinkedListNode<Range>? currentNode = tempRanges.First;

            if (currentNode is null)
            {
                return;
            }

            do
            {

                var nextNode = currentNode.Next;
                if (nextNode != null)
                {
                    if (nextNode.Value.Start <= currentNode.Value.End)
                    {
                        if (nextNode.Value.End > currentNode.Value.End)
                        {
                            currentNode.Value.End = nextNode.Value.End;
                        }

                        tempRanges.Remove(nextNode);
                    }
                    else
                    {
                        currentNode = nextNode;
                    }

                }

            } while (currentNode.Next != null);

            ConsolidatedRanges = tempRanges.ToList();

        }

        public int CountFresh()
        {
            int count = 0;

            foreach (var item in Items)
            {
                if (ConsolidatedRanges.Any(o => o.IsInRange(item)))
                {
                    count++;
                }
            }

            return count;
        }

        public long CountFreshIds()
        {
            long count = 0;

            foreach (var range in ConsolidatedRanges)
            {
                count += range.Length;
            }

            return count;
        }

        private class Range
        {
            public long Start { get; set; }
            public long End { get; set; }

            public bool IsInRange(long value)
            {
                return Start <= value && value <= End;
            }

            public long Length => End - Start + 1;

        }
    }
}
