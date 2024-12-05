using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode24.Day04
{
    internal class Day04
    {
        public Day04()
        {
            var input = File.ReadAllLines(@"day04\input1.txt").ToList();
            Console.WriteLine(part1(input));
            Console.WriteLine(part2(input));
        }

        public int part2(List<string> input)
        {
            var count = 0;

            for (var i = 0; i < 4; i++)
            {
                for (var iLine = 0; iLine < input.Count - 2; iLine++)
                {
                    for (var iCol = 0; iCol < input[iLine].Length - 2; iCol++)
                    {
                        // detect top left to bottom right MAS
                        if (input[iLine][iCol] == 'M'
                            && input[iLine + 1][iCol + 1] == 'A'
                            && input[iLine + 2][iCol + 2] == 'S')
                        {
                            // detect bottom left to top right MAS
                            if (input[iLine + 2][iCol] == 'M'
                                && input[iLine + 1][iCol + 1] == 'A'
                                && input[iLine][iCol + 2] == 'S')
                            {
                                count++;
                            }
                        }
                    }
                }

                input = rotate90(input);
            }

            return count;
        }

        public int part1(List<string> input)
        {
            var count = 0;

            for (var i = 0; i < 4; i++)
            {
                // detect horizontal
                foreach (var line in input)
                {
                    // detect horizontal
                    count += Regex.Matches(line, "XMAS").Count();
                }

                // detect vertical
                for (var iLine = 0; iLine < input.Count - 3; iLine++)
                {
                    for (var iCol = 0; iCol < input[iLine].Length - 3; iCol++)
                    {
                        if (input[iLine][iCol] == 'X'
                            && input[iLine + 1][iCol + 1] == 'M'
                            && input[iLine + 2][iCol + 2] == 'A'
                            && input[iLine + 3][iCol + 3] == 'S'
                            )
                        {
                            count++;
                        }
                    }
                }

                input = rotate90(input);
            }

            return count;
        }


        private List<string> rotate90(List<string> input)
        {
            List<string> output = new List<string>();

            for (int iCol = 0; iCol < input[0].Length; iCol++)
            {
                char[] newRow = new char[input.Count];
                for (int iRow = 0; iRow < input.Count; iRow++)
                {
                    newRow[iRow] = input[input.Count - iRow - 1][iCol];
                }
                output.Add(new string(newRow));
            }

            return output;
        }
    }
}
