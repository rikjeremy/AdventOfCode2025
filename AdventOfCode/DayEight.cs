using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    internal class DayEight
    {
        private readonly List<Coordinate3d> _junctionBoxes = [];

        public DayEight()
        {
            using var fs = new FileStream($"{AppContext.BaseDirectory}files/input-day8.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line != null)
                {
                    _junctionBoxes.Add(new Coordinate3d(line));
                }
            }
        }

        public long ConnectLights1()
        {
            HashSet<Link> _possibleConnections = [];
            var currentIndex = 1;
            HashSet<Circuit> _circuits = [];

            foreach (var jb in _junctionBoxes)
            {
                if (currentIndex < _junctionBoxes.Count)
                {
                    _possibleConnections.UnionWith(_junctionBoxes[currentIndex..].Select(o => new Link { Start = jb, End = o }));
                }
                currentIndex++;
            }

            foreach (var link in _possibleConnections.OrderBy(o => o.Length).Take(1000)) // take should be 10 for demo dataset
            {
                if (_circuits.Count == 0)
                {
                    _circuits.Add(new Circuit(link));
                    continue;
                }

                List<Circuit> potentialCircuits = [];

                foreach (var existingCircuit in _circuits)
                {
                    if (existingCircuit.LinksTo(link))
                    {
                        potentialCircuits.Add(existingCircuit);
                    }
                }

                switch (potentialCircuits.Count)
                {
                    case 0:
                        {
                            _circuits.Add(new Circuit(link));
                            break;
                        }
                    case 1:
                        {
                            potentialCircuits[0].AddLink(link);
                            break;
                        }
                    case 2:
                        {
                            Circuit.Join(potentialCircuits[0], potentialCircuits[1], link);
                            break;
                        }
                    default:
                        {
                            throw new InvalidOperationException();
                        }
                }
                _circuits.RemoveWhere(o => o.Removed);
            }

            var largestCircuits = _circuits.OrderByDescending(o => o.JunctionCount).Take(3);

            var answer = largestCircuits.Select(o => (long)o.JunctionCount).Aggregate(1, (long a, long b) => a * b);

            return answer;

        }

        public long ConnectLights2()
        {
            HashSet<Link> _possibleConnections = [];
            var currentIndex = 1;
            HashSet<Circuit> _circuits = [];

            foreach (var jb in _junctionBoxes)
            {
                if (currentIndex < _junctionBoxes.Count)
                {
                    _possibleConnections.UnionWith(_junctionBoxes[currentIndex..].Select(o => new Link { Start = jb, End = o }));
                }
                currentIndex++;
            }

            Link? lastLink = null;
            foreach (var link in _possibleConnections.OrderBy(o => o.Length))
            {
                lastLink = link;
                if (_circuits.Count == 0)
                {
                    _circuits.Add(new Circuit(link));
                    continue;
                }

                List<Circuit> potentialCircuits = [];

                foreach (var existingCircuit in _circuits)
                {
                    if (existingCircuit.LinksTo(link))
                    {
                        potentialCircuits.Add(existingCircuit);
                    }
                }

                switch (potentialCircuits.Count)
                {
                    case 0:
                        {
                            _circuits.Add(new Circuit(link));
                            break;
                        }
                    case 1:
                        {
                            potentialCircuits[0].AddLink(link);
                            break;
                        }
                    case 2:
                        {
                            Circuit.Join(potentialCircuits[0], potentialCircuits[1], link);
                            break;
                        }
                    default:
                        {
                            throw new InvalidOperationException();
                        }
                }

                _circuits.RemoveWhere(o => o.Removed);

                if (_circuits.Count == 1 && !_junctionBoxes.Except(_circuits.First().AllJunctions).Any())
                {
                    break;
                }
            }

            return lastLink!.Start.X * lastLink!.End.X;
        }

        private class Circuit
        {
            public Circuit(Link initialLink)
            {
                AddLink(initialLink);
            }

            private HashSet<Link> _links { get; set; } = [];

            private HashSet<Coordinate3d> _junctions { get; set; } = [];

            public List<Link> AllLinks => _links.ToList();

            public HashSet<Coordinate3d> AllJunctions => _junctions.ToHashSet();

            public void AddLink(Link link)
            {
                _links.Add(link);
                RecalculateJunctions();
            }

            public void AddLinks(IEnumerable<Link> links)
            {
                _links.UnionWith(links);
                RecalculateJunctions();
            }

            public bool LinksTo(Link link) => _links.Where(o => o.CanLink(link)).Any();

            public static void Join(Circuit a, Circuit b, Link connector)
            {
                var mainCircuit = a.JunctionCount >= b.JunctionCount ? a : b;
                var otherCircuit = a.JunctionCount >= b.JunctionCount ? b : a;

                mainCircuit.AddLink(connector);
                mainCircuit.AddLinks(otherCircuit.AllLinks);
                otherCircuit.Remove();
            }

            public void Remove()
            {
                _links.Clear();
                RecalculateJunctions();
            }

            private void RecalculateJunctions()
            {
                _junctions = _links.Select(o => new List<Coordinate3d> { o.Start, o.End }).SelectMany(o => o).ToHashSet();
            }

            public int JunctionCount => _junctions.Count;

            public bool Removed => _links.Count == 0;
        }

        private class Link
        {
            public required Coordinate3d Start { get; set; }
            public required Coordinate3d End { get; set; }

            public decimal Length => Math.Abs((decimal)Math.Sqrt(Math.Pow(Start.X - End.X, 2) + Math.Pow(Start.Y - End.Y, 2) + Math.Pow(Start.Z - End.Z, 2)));

            public bool CanLink(Link link)
            {
                return (Start == link.Start) || (Start == link.End) || (End == link.Start) || (End == link.End);
            }

            public override int GetHashCode()
            {
                return Start.GetHashCode() ^ End.GetHashCode();
            }

            public override bool Equals(object? obj)
            {
                if (obj is not Link comp)
                {
                    return false;
                }

                return (Start == comp.Start && End == comp.End) || (End == comp.Start && Start == comp.End);
            }
        }

        private class Coordinate3d
        {
            public Coordinate3d() { }

            public Coordinate3d(string importLine)
            {
                string[] coords = importLine.Split(',');
                if (coords.Length != 3)
                {
                    throw new ArgumentException();
                }
                X = int.Parse(coords[0]);
                Y = int.Parse(coords[1]);
                Z = int.Parse(coords[2]);
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public override int GetHashCode()
            {
                return X ^ (Y * 609923) ^ (Z * 611027);
            }

            public override bool Equals(object? obj)
            {
                if (obj is not Coordinate3d comp)
                {
                    return false;
                }

                return X == comp.X && Y == comp.Y && Z == comp.Z;
            }
        }
    }

}
