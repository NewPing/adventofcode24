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
            var levels = File.ReadAllLines(@"day02\input1.txt").Select(x => x.Split(" ").Select(x => int.Parse(x)).ToList()).ToList();
            Console.WriteLine("playboy: "+ part1A(levels));
            Console.WriteLine("playboy p2: " + part2A(levels));

            Console.WriteLine("playbitch: " + partJ(levels, false));
            Console.WriteLine("playbitch p2: " + partJ(levels, true));
        }
        public int part1A(List<List<int>> input)
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

        public int part2A(List<List<int>> input)
        {
            var counter = 0;
            foreach (var row in input)
            {
                if (validRow(row, true, true) || validRow(row, false, true))
                {
                    counter++;
                }
                else
                {

                }
            }
            return counter;
        }
        private bool validRow(List<int> row, bool ascending, bool damper)
        {
            //start
            if (!validPair(row[0], row[1], ascending))
            {
                if (!damper || !validPair(row[1], row[2], ascending))
                {
                    return false;
                }
                damper = false;
            }
            //middle
            for (var i = 1; i < row.Count - 2; i++)
            {
                if (!validPair(row[i], row[i + 1], ascending))
                {                                                                //this is retarded edge case
                    if (!damper || (!validPair(row[i], row[i + 2], ascending) && !validPair(row[i - 1], row[i + 1], ascending)))
                    {
                        return false;
                    }
                    damper = false;
                }
            }
            //end
            if (!validPair(row[row.Count - 2], row[row.Count - 1], ascending))
            {

                if (!damper || !validPair(row[row.Count - 3], row[row.Count - 2], ascending))
                {
                    return false;
                }
                damper = false;

            }
            return true;
        }

        private bool validPair(int value1, int value2, bool ascending)
        {
            if ((value1 < value2 && !ascending) || (value1 > value2 && ascending) || Math.Abs(value1 - value2) > 3 || Math.Abs(value1 - value2) < 1)
            {
                return false;
            }
            return true;
        }

        //--------------------------------------J VS A-------------------------------------------//


        private int partJ(List<List<int>> input, bool useTolerance)
        {
            var resultCount = 0;
            foreach (var row in input)
            {
                var steppingResult = isSteppingLegal(row, useTolerance);
                if (steppingResult.Item1)
                {
                    if (isSteppingContinues(row, isIncreasing: true, useTolerance, steppingResult.Item2))
                    {
                        resultCount++;
                    }
                    else if (isSteppingContinues(row, isIncreasing: false, useTolerance, steppingResult.Item2))
                    {
                        resultCount++;
                    }
                }
            }
            return resultCount;
        }

        private (bool, int) isSteppingLegal(List<int> inputs, bool useTolerance)
        {
            var toleranceUsed = useTolerance ? false : true;
            var toleranceIndex = -1;
            for (var i = 0; i < inputs.Count - 1; i++)
            {
                var difference = Math.Abs(inputs[i] - inputs[i + 1]);
                if (difference == 0 || difference > 3)
                {
                    if (toleranceUsed)
                    {
                        return (false, toleranceIndex);
                    }
                    toleranceUsed = true;
                    toleranceIndex = i;
                    if (i != 0)
                    {
                        inputs[i + 1] = inputs[i];
                    }
                }
            }
            return (true, toleranceIndex);
        }

        private bool isSteppingContinues(List<int> inputs, bool isIncreasing, bool useTolerance, int toleranceIndex)
        {
            toleranceIndex = useTolerance ? toleranceIndex : -2;
            for (var i = 0; i < inputs.Count - 1; i++)
            {
                if (i == toleranceIndex && i != 0)
                {
                    inputs[i + 1] = inputs[i];
                    continue;
                }
                if (!((isIncreasing && (inputs[i] > inputs[i + 1])) || (!isIncreasing && (inputs[i] < inputs[i + 1]))))
                {
                    if (toleranceIndex != -1)
                    {
                        return false;
                    }
                    toleranceIndex = i;
                    if (i != 0)
                    {
                        inputs[i + 1] = inputs[i];
                    }
                }
            }
            return true;
        }
    }
}
