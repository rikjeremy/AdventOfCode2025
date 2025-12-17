using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal class DaySeven
    {
        private List<List<char>> plan = [];
        private List<List<char>> runPlan = [];

        public DaySeven()
        {
            string input;

            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day7.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                input = sr.ReadLine()?.Trim() ?? string.Empty;
                plan.Add(input.ToCharArray().ToList());
            }
        }

        public int RunSplit()
        {
            HashSet<int> currentColumns = [];
            var splits = 0;

            var first = true;

            foreach (var step in plan)
            {
                var runstep = step.ToList();

                if (first)
                {
                    runPlan.Add(runstep);
                    for (int i = 0; i < runstep.Count; i++)
                    {
                        if (runstep[i] == 'S')
                        {
                            currentColumns.Add(i);
                        }
                    }
                    first = false;
                    continue;
                }

                HashSet<int> splitters = [];
                for (int i = 0; i < runstep.Count; i++)
                {
                    if (runstep[i] == '^')
                    {
                        splitters.Add(i);
                    }
                }

                if (splitters.Any())
                {
                    foreach (var splitter in splitters)
                    {
                        if (currentColumns.Contains(splitter))
                        {
                            splits++;
                        }
                        currentColumns.Remove(splitter);

                        if (splitter < runstep.Count - 1)
                        {
                            currentColumns.Add(splitter + 1);
                        }

                        if (splitter > 0)
                        {
                            currentColumns.Add(splitter - 1);
                        }
                    }
                }

                foreach (var idx in currentColumns)
                {
                    runstep[idx] = '|';
                }
                runPlan.Add(runstep);
            }

            foreach (var step in runPlan)
            {
                Console.WriteLine(new string([.. step]));
            }

            return splits;
        }

        public long GenerateTimelines()
        {
            HashSet<Column> currentColumns = [];

            var first = true;

            foreach (var step in plan)
            {
                var runstep = step.ToList();

                if (first)
                {
                    runPlan.Add(runstep);
                    for (int i = 0; i < runstep.Count; i++)
                    {
                        if (runstep[i] == 'S')
                        {
                            currentColumns.Add(new Column { ColumnId = i, Routes = 1 });
                        }
                    }
                    first = false;
                    continue;
                }

                HashSet<int> splitters = [];
                for (int i = 0; i < runstep.Count; i++)
                {
                    if (runstep[i] == '^')
                    {
                        splitters.Add(i);
                    }
                }

                if (splitters.Any())
                {
                    foreach (var splitter in splitters)
                    {
                        var splitterCol = currentColumns.Where(o => o.ColumnId == splitter).FirstOrDefault();
                        if (splitterCol is null)
                        {
                            continue;
                        }

                        if (splitter < runstep.Count - 1)
                        {
                            var upperCol = currentColumns.Where(o => o.ColumnId == splitter + 1).FirstOrDefault();
                            if (upperCol is not null)
                            {
                                upperCol.Routes += splitterCol.Routes;
                            }
                            else
                            {
                                currentColumns.Add(new Column { ColumnId = splitter + 1, Routes = splitterCol.Routes });
                            }
                        }

                        if (splitter > 0)
                        {
                            var lowerCol = currentColumns.Where(o => o.ColumnId == splitter - 1).FirstOrDefault();
                            if (lowerCol is not null)
                            {
                                lowerCol.Routes += splitterCol.Routes;
                            }
                            else
                            {
                                currentColumns.Add(new Column { ColumnId = splitter - 1, Routes = splitterCol.Routes });
                            }
                        }

                        currentColumns.Remove(splitterCol);
                    }
                }

                foreach (var idx in currentColumns.Select(o => o.ColumnId))
                {
                    runstep[idx] = '|';
                }
                runPlan.Add(runstep);
            }

            foreach (var step in runPlan)
            {
                Console.WriteLine(new string([.. step]));
            }

            return currentColumns.Sum(o => o.Routes);
        }

        private class Column
        {
            public long Routes { get; set; }
            public int ColumnId { get; set; }

            public override int GetHashCode()
            {
                return ColumnId;
            }

            public override bool Equals(object? obj)
            {
                if (!(obj is Column)) return false;

                Column c = (Column)obj;
                return ColumnId == c.ColumnId && Routes == c.Routes;
            }
        }
    }
}
