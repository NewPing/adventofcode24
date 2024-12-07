using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode24.Day07
{
    internal class Day07
    {
        public Day07()
        {
            var inputLines = File.ReadAllLines(@"day07\input1.txt");

            var input = new List<(BigInteger, List<BigInteger>)>();
            foreach (var line in inputLines)
            {
                var p1 = BigInteger.Parse(Regex.Match(line.Split(":").First(), @"\d+").Value);
                var p2 = Regex.Matches(line.Split(":").Last(), @"\d+").Select(x => BigInteger.Parse(x.Value)).ToList();
                input.Add((p1, p2));
            }

            Console.WriteLine(part1(input));
            Console.WriteLine(part2(input));
        }

        private BigInteger part1(List<(BigInteger, List<BigInteger>)> input)
        {
            BigInteger count = 0;
            foreach (var line in input)
            {
                List<string> path = new List<string>();
                var isSolveable = tryCombinations(line.Item1, line.Item2, 1, line.Item2[0]);
                if (isSolveable)
                {
                    count += line.Item1;
                }
            }

            return count;
        }

        private BigInteger part2(List<(BigInteger, List<BigInteger>)> input)
        {
            BigInteger count = 0;
            foreach (var line in input)
            {
                List<string> path = new List<string>();
                var isSolveable = tryCombinationsP2(line.Item1, line.Item2, 1, line.Item2[0], path);
                if (isSolveable)
                {
                    count += line.Item1;
                }
            }

            return count;
        }

        /// <summary>
        /// recursively tries to reach the target by adding or multiplying the current result with the next number
        /// </summary>
        /// <param name="target">the number to achieve</param>
        /// <param name="numbers">the numbers to use in the equation</param>
        /// <param name="index">the current index in the array</param>
        /// <param name="currentValue">the current value of the calculation</param>
        /// <returns>true if the target can be reached, otherwise false</returns>
        private bool tryCombinations(BigInteger target, List<BigInteger> numbers, int index, BigInteger currentValue)
        {
            if (index == numbers.Count)
            {
                return currentValue == target;
            }

            var nextNumber = numbers[index];

            var addResult = tryCombinations(target, numbers, index + 1, currentValue + nextNumber);
            var multiplyResult = tryCombinations(target, numbers, index + 1, currentValue * nextNumber);

            return addResult || multiplyResult;
        }

        private bool tryCombinationsP2(BigInteger target, List<BigInteger> numbers, int index, BigInteger currentValue, List<string> path)
        {
            if (index == numbers.Count)
            {
                if (currentValue == target)
                {
                    return true;
                }
                return false;
            }

            var nextNumber = numbers[index];

            path.Add($"+ {nextNumber}");
            if (tryCombinationsP2(target, numbers, index + 1, currentValue + nextNumber, path))
            {
                return true;
            }
            path.RemoveAt(path.Count - 1);

            path.Add($"* {nextNumber}");
            if (tryCombinationsP2(target, numbers, index + 1, currentValue * nextNumber, path))
            {
                return true;
            }
            path.RemoveAt(path.Count - 1);

            var concatenatedValue = BigInteger.Parse($"{currentValue}{nextNumber}");
            path.Add($"|| {nextNumber}");
            if (tryCombinationsP2(target, numbers, index + 1, concatenatedValue, path))
            {
                return true;
            }
            path.RemoveAt(path.Count - 1);

            return false;
        }
    }
}
