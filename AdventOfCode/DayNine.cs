using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class DayNine
    {
        private List<Coordinate2d> _coordinates = [];

        public DayNine()
        {
            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day9.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line != null)
                {
                    _coordinates.Add(new Coordinate2d(line));
                }
            }
        }

        public long GetLargestArea()
        {
            HashSet<GridArea> areas = [];
            int currentIndex = 1;

            foreach (var coord in _coordinates)
            {
                if (currentIndex < _coordinates.Count)
                {
                    areas.UnionWith(_coordinates[currentIndex..].Select(o => new GridArea { FirstCoordinate = coord, LastCoordinate = o }));
                }
                currentIndex++;
            }

            return areas.Max(o => o.Area);

        }

        private class Coordinate2d
        {
            public Coordinate2d() { }

            public Coordinate2d(string importLine)
            {
                string[] coords = importLine.Split(',');
                if (coords.Length != 2)
                {
                    throw new ArgumentException();
                }
                X = long.Parse(coords[0]);
                Y = long.Parse(coords[1]);
            }
            public long X { get; set; }
            public long Y { get; set; }

            public override int GetHashCode()
            {
                return (int)X ^ ((int)Y * 609923);
            }

            public override bool Equals(object? obj)
            {
                if (obj is not Coordinate2d comp)
                {
                    return false;
                }

                return X == comp.X && Y == comp.Y;
            }
        }

        private class GridArea
        {
            public required Coordinate2d FirstCoordinate { get; set; }
            public required Coordinate2d LastCoordinate { get; set; }

            public long Area
            {
                get
                {
                    // add one as coordinates are inclusive
                    var xLen = Math.Abs(FirstCoordinate.X - LastCoordinate.X) + 1;
                    var yLen = Math.Abs(FirstCoordinate.Y - LastCoordinate.Y) + 1;

                    return xLen * yLen;
                }
            }

            public override int GetHashCode()
            {
                return FirstCoordinate.GetHashCode() ^ LastCoordinate.GetHashCode();
            }

            public override bool Equals(object? obj)
            {
                if (obj is not GridArea comp)
                {
                    return false;
                }

                return (FirstCoordinate == comp.FirstCoordinate && LastCoordinate == comp.LastCoordinate) || (LastCoordinate == comp.FirstCoordinate && FirstCoordinate == comp.LastCoordinate);
            }
        }
    }
}
