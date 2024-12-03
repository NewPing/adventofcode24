using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode24.Day03
{
    internal class Day03
    {
        public Day03()
        {
            var inputText = File.ReadAllText(@"day03\exampleInput.txt");
            var inputText2 = File.ReadAllText(@"day03\input2.txt");
            Console.WriteLine(part1(inputText));
            Console.WriteLine(part2(inputText2));
        }

        public int part1(string inputText)
        {
            var inputs = Regex.Matches(inputText, @"mul\(\d+,\d+\)").Select(x => Regex.Matches(x.Value, @"\d+").Select(x => int.Parse(x.Value)).ToList()).ToList();
            var result = 0;
            foreach (var input in inputs)
            {
                result += input.First() * input.Last();
            }
            return result;
        }

        public int part2(string inputText)
        {
            var muls = Regex.Matches(inputText, @"mul\(\d+,\d+\)").Select(x => (x.Index, Regex.Matches(x.Value, @"\d+").Select(x => int.Parse(x.Value)).ToList())).ToList();
            var indexDo = Regex.Matches(inputText, @"do\(\)").Select(x => (x.Index, true)).ToList();
            var indexDont = Regex.Matches(inputText, @"don't\(\)").Select(x => (x.Index, false)).ToList();
            var indicators = indexDo.Concat(indexDont).OrderBy(x => x.Item1).ToList();

            var enabledRanges = new List<(int, int)>();

            var lastEnabledIndex = 0;
            foreach (var indicator in indicators)
            {
                if (!indicator.Item2)
                {
                    enabledRanges.Add((lastEnabledIndex, indicator.Item1));
                } else
                {
                    lastEnabledIndex = indicator.Item1;
                    if (indicators.Last() == indicator)
                    {
                        enabledRanges.Add((indicator.Item1, int.MaxValue));
                    }
                }
            }

            var result = 0;
            foreach (var mul in muls)
            {
                if (isInRangeList(enabledRanges, mul.Item1))
                {
                    result += mul.Item2.First() * mul.Item2.Last();
                }
            }

            return result;
        }

        public bool isInRangeList(List<(int, int)> ranges, int n)
        {
            foreach (var range in ranges)
            {
                if (range.Item1 <= n && n <= range.Item2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
