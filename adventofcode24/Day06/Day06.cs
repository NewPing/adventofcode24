using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adventofcode24.Day06
{
    internal class Day06
    {
        public Day06()
        {
            var input = File.ReadAllLines(@"day06\input1.txt").Select(x => x.ToCharArray().Select(y => y.ToString()).ToList()).ToList();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine(part1(input)); //already takes 2-6 seconds... part2 must be an optimization problem
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            Console.WriteLine(part2(input)); //f***, its an optimization problem...
        }

        private int part2(List<List<string>> input)
        {
            return 0;
        }

        private int part1(List<List<string>> input)
        {
            var isGuardOnField = true;
            while (isGuardOnField)
            {
                isGuardOnField = makeMove(input);
            }

            var counter = 0;
            foreach (var row in input)
            {
                foreach (var cell in row)
                {
                    if (cell == "X")
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        private bool makeMove(List<List<string>> input)
        {
            var guardInfo = getGuardInfo(input);
            switch (guardInfo.Direction)
            {
                case GuardDirection.Up:
                    input[guardInfo.PosY][guardInfo.PosX] = "X";
                    if (guardInfo.PosY == 0)
                    {
                        return false;
                    }
                    if (input[guardInfo.PosY - 1][guardInfo.PosX] == "#")
                    {
                        input[guardInfo.PosY][guardInfo.PosX + 1] = ">";
                    } else
                    {
                        input[guardInfo.PosY - 1][guardInfo.PosX] = "^";
                    }
                    break;
                case GuardDirection.Right:
                    input[guardInfo.PosY][guardInfo.PosX] = "X";
                    if (guardInfo.PosX == input[guardInfo.PosY].Count -1)
                    {
                        return false;
                    }
                    if (input[guardInfo.PosY][guardInfo.PosX + 1] == "#")
                    {
                        input[guardInfo.PosY + 1][guardInfo.PosX] = "v";
                    }
                    else
                    {
                        input[guardInfo.PosY][guardInfo.PosX + 1] = ">";
                    }
                    break;
                case GuardDirection.Left:
                    input[guardInfo.PosY][guardInfo.PosX] = "X";
                    if (guardInfo.PosX == 0)
                    {
                        return false;
                    }
                    if (input[guardInfo.PosY][guardInfo.PosX - 1] == "#")
                    {
                        input[guardInfo.PosY - 1][guardInfo.PosX] = "^";
                    }
                    else
                    {
                        input[guardInfo.PosY][guardInfo.PosX - 1] = "<";
                    }
                    break;
                case GuardDirection.Down:
                    input[guardInfo.PosY][guardInfo.PosX] = "X";
                    if (guardInfo.PosY == input.Count -1)
                    {
                        return false;
                    }
                    if (input[guardInfo.PosY + 1][guardInfo.PosX] == "#")
                    {
                        input[guardInfo.PosY][guardInfo.PosX - 1] = "<";
                    }
                    else
                    {
                        input[guardInfo.PosY + 1][guardInfo.PosX] = "v";
                    }
                    break;
                default: 
                    throw new NotImplementedException("move not implemented...");
            }

            return true;
        }

        private GuardInfo getGuardInfo(List<List<string>> input)
        {
            for (var iRow = 0; iRow < input.Count; iRow++)
            {
                for (var iCol = 0; iCol < input[iRow].Count; iCol++)
                {
                    var cell = input[iRow][iCol];
                    if ((new string[]{ "^", ">", "<", "v" }).Contains(cell))
                    {
                        var guardDirection = GuardDirection.Up;
                        switch (cell)
                        {
                            case "^":
                                guardDirection = GuardDirection.Up;
                                break;
                            case ">":
                                guardDirection = GuardDirection.Right;
                                break;
                            case "<":
                                guardDirection = GuardDirection.Left;
                                break;
                            case "v":
                                guardDirection = GuardDirection.Down;
                                break;
                            default:
                                throw new NotImplementedException("Guard Direction not implemented yet...");
                        }
                        return new GuardInfo(iCol, iRow, guardDirection);
                    }
                }
            }

            //return new GuardInfo(-1, -1, GuardDirection.Up);
            throw new NotImplementedException("unable to find guard...");
        }

    }

    public class GuardInfo
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public GuardDirection Direction { get; set; }

        public GuardInfo(int _posX, int _posY, GuardDirection _direction)
        {
            PosX = _posX;
            PosY = _posY;
            Direction = _direction;
        }
    }

    public enum GuardDirection
    {
        Up, Down, Left, Right
    }
}