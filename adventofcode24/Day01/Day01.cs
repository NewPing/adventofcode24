using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode24.Day1
{
    public class Day01
    {
        public Day01()
        {
            var lines = File.ReadAllLines(@"day01\input1.txt").Select(x => Regex.Matches(x, @"\d+").Select(x => int.Parse(x.Value)).ToList()).ToList();
            var values1 = lines.Select(x => x.First()).OrderBy(x => x).ToList();
            var values2 = lines.Select(x => x.Last()).OrderBy(x => x).ToList();

            part1(values1, values2);
            part2(values1, values2);
        }

        public void part1(List<int> input1, List<int> input2)
        {
            int endValue = 0;
            for (int i = 0; i < input1.Count; i++)
            {
                endValue += Math.Abs(input1[i] - input2[i]);

            }
            Console.WriteLine(endValue);
        }

        public void part2(List<int> input1, List<int> input2)
        {
            int similiarSum = input1.Select(x => x * input2.Where(y => y == x).Count()).Sum();
            Console.WriteLine(similiarSum);
        }

    }
}
