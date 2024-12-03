using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode24.Day02
{
    internal class Day02
    {
        public Day02()
        {
            var levels = File.ReadAllLines(@"day02\input2.txt").Select(x => x.Split(" ").Select(x => int.Parse(x)).ToList()).ToList();
            Console.WriteLine("playboy p1: "+ part1(levels));
            Console.WriteLine("playboy p2: " + part2(levels));
        }
        public int part1(List<List<int>> input)
        {
            var counter = input.Count;
            var ascending = false;
            foreach (var row in input)
            {
                for (var i = 0; i < row.Count - 1; i++)
                {
                    ascending = row[0] <= row[1];
                    if(row[i] == row[i + 1])
                    {
                        counter--;
                        break;
                    }
                    if ((row[i] > row[i + 1] && ascending) || Math.Abs(row[i] - row[i + 1]) > 3)
                    {   
                        counter--;
                        break;
                    }
                    if ((row[i] < row[i + 1] && !ascending) || Math.Abs(row[i] - row[i + 1]) > 3)
                    {
                        counter--;
                        break;
                    }

                }
            }
            return counter;
        }


        private int part2(List<List<int>> input)
        {
            var resultCount = 0;
            foreach (var row in input)
            {
                if (isRowLegal(row))
                {
                    resultCount++;
                }
            }
            return resultCount;
        }

        private bool isRowLegal(List<int> row)
        {
            for (int i = 0; i < row.Count(); i++)
            {
                var r = row.ToList();
                r.RemoveAt(i);
                if (isLegal(r))
                {
                    return true;
                }
            }
            return false;
        }

        private bool isLegal(List<int> row)
        {
            if (isSteppingLegal(row))
            {
                if (isSteppingContinues(row, isIncreasing: true))
                {
                    return true;
                }
                else if (isSteppingContinues(row, isIncreasing: false))
                {
                    return true;
                }
            }
            return false;
        }

        private bool isSteppingLegal(List<int> inputs)
        {
            for (var i = 0; i < inputs.Count - 1; i++)
            {
                var difference = Math.Abs(inputs[i] - inputs[i + 1]);
                if (difference == 0 || difference > 3)
                {
                    return false;
                }
            }
            return true;
        }

        private bool isSteppingContinues(List<int> inputs, bool isIncreasing)
        {
            for (var i = 0; i < inputs.Count - 1; i++)
            {
                if (!((isIncreasing && (inputs[i] > inputs[i + 1])) || (!isIncreasing && (inputs[i] < inputs[i + 1]))))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
