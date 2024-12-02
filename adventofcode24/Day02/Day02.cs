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
            var counter = input.Count;
            var ascending = false;
            foreach (var row in input)
            {
                bool damper = true;
                var rand = new Random();
                var dir = getSortDirection(rand, row, 5);
                ascending = dir == 1 ? true : false;
                if (dir == 0)
                {
                    Console.WriteLine("ascending?: " + ascending + string.Join(", ", row));
                    counter--;
                    continue;
                }
                for (var i = 0; i < row.Count - 1; i++)
                {
                    if ( (row[i] < row[i + 1] && !ascending) || (row[i] > row[i + 1] && ascending) || Math.Abs(row[i] - row[i + 1]) > 3 || Math.Abs(row[i] - row[i + 1]) < 1)
                    {
                        //Console.WriteLine("wrong: "+ ascending+ string.Join(", ", row));
                        if (!damper || i>=row.Count-2)
                        {
                            counter--;
                            break;
                        }
                        if ((row[i] < row[i + 2] && !ascending) || (row[i] > row[i + 2] && ascending) || Math.Abs(row[i] - row[i + 2]) > 3 || Math.Abs(row[i] - row[i + 2]) < 1)
                        {
                            //Console.WriteLine("wrong damper: " + string.Join(", ", row));
                            counter--;
                            break;
                        }
                        damper = false;
                    }
                }
            }
            return counter;
        }

        public (bool, bool) giveAsc(List<int> numbers)
        {
            var numbers2 = new List<int>(numbers);
            int middleIndex = numbers2.Count / 2;
            var firstHalf = numbers2.Take(middleIndex).ToList();
            var secondHalf = numbers2.Skip(middleIndex).ToList();
            int sumFirstHalf = firstHalf.Sum();
            int sumSecondHalf = secondHalf.Sum();
            if(sumFirstHalf == sumSecondHalf)
            {
                return (true, false);
            }
            return (sumFirstHalf < sumSecondHalf, true);
        }

        private int getSortDirection(Random rand, List<int> inputs, int samples)
        {
            var rn = rand.Next(0, inputs.Count - 2);
            var incCount = 0;
            var decCount = 0;
            var sameCount = 0;
            for (var i = 0; i < samples; i++)
            {
                if (inputs[rn] > inputs[rn + 1])
                {
                    incCount++;
                }
                else if (inputs[rn] < inputs[rn + 1])
                {
                    decCount++;
                }
                else
                {
                    sameCount++;
                }
                rn = rand.Next(0, inputs.Count - 2);
            }

            if (incCount > decCount && incCount > sameCount)
            {
                return 1;
            }
            else if (decCount > incCount && decCount > sameCount)
            {
                return 2;
            }
            else
            {
                return 0;
            }
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
