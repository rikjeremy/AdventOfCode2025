using AdventOfCode;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        //var d1 = new DayOne();
        //Console.WriteLine(d1.GetPassword());
        //Console.WriteLine(d1.GetPassword2());

        //var d2 = new DayTwo();
        //Console.WriteLine(d2.GetSumOfInvalid());
        //Console.WriteLine(d2.GetSumOfInvalid2());

        //var d3 = new DayThree();
        //Console.WriteLine(d3.GetTotal());
        //Console.WriteLine(d3.GetTotal2());

        //var d4 = new DayFour();
        //Console.WriteLine(d4.CountAccessible());
        //Console.WriteLine(d4.CountAccessible2());

        //var d5 = new DayFive();
        //Console.WriteLine(d5.CountFresh());
        //Console.WriteLine(d5.CountFreshIds());

        //var d6 = new DaySix();
        //Console.WriteLine(d6.GetCheckTotal(1));
        //Console.WriteLine(d6.GetCheckTotal(2));

        var d7 = new DaySeven();
        Console.WriteLine(d7.RunSplit());
        Console.WriteLine(d7.GenerateTimelines());
    }
}