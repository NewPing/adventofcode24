using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace adventofcode24.Day06
{
    internal class Day06
    {
        public Day06()
        {
            var inputFilePath = @"day06\input1.txt";

            var input = File.ReadAllLines(inputFilePath).Select(x => x.ToCharArray().ToList()).ToList();
            Console.WriteLine(part1(input));

            input = File.ReadAllLines(inputFilePath).Select(x => x.ToCharArray().ToList()).ToList();
            Console.WriteLine(part2(input));
        }

        private int part2(List<List<char>> input)
        {
            var isGuardOnField = true;
            var guardInfo = getGuardInfo(input);
            while (isGuardOnField)
            {
                isGuardOnField = makeMove(input, guardInfo).Item1;
            }

            var counter = 0;
            foreach (var row in input)
            {
                foreach (var cell in row)
                {
                    if (cell == 'X')
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }
        
        private bool isGuardLooping(Dictionary<string, GuardInfo> pastGuardInfos, GuardInfo currentGuardInfo)
        {
            return pastGuardInfos.ContainsKey(currentGuardInfo.ToString());
        }

        private int part1(List<List<char>> input)
        {
            var isGuardOnField = true;
            var guardInfo = getGuardInfo(input);
            while (isGuardOnField)
            {
                isGuardOnField = makeMove(input, guardInfo).Item1;
            }

            var counter = 0;
            foreach (var row in input)
            {
                foreach (var cell in row)
                {
                    if (cell == 'X')
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        /// <summary>
        /// makes a move on the current input. if the guard goes outside of the field, it returns false; otherwise true
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private (bool, GuardInfo) makeMove(List<List<char>> input, GuardInfo guardInfo)
        {
            var newGuardDirection = getNewGuardDirection(input, guardInfo);
            guardInfo.Direction = newGuardDirection.Item1;
            var isNextMoveValid = newGuardDirection.Item2;

            input[guardInfo.PosY][guardInfo.PosX] = 'X';

            if (isNextMoveValid)
            {
                switch (guardInfo.Direction)
                {
                    case GuardDirection.Up:
                        input[guardInfo.PosY - 1][guardInfo.PosX] = '^';
                        guardInfo.PosY--;
                        break;
                    case GuardDirection.Right:
                        input[guardInfo.PosY][guardInfo.PosX + 1] = '>';
                        guardInfo.PosX++;
                        break;
                    case GuardDirection.Left:
                        input[guardInfo.PosY][guardInfo.PosX - 1] = '<';
                        guardInfo.PosX--;
                        break;
                    case GuardDirection.Down:
                        input[guardInfo.PosY + 1][guardInfo.PosX] = 'v';
                        guardInfo.PosY++;
                        break;
                    default:
                        throw new NotImplementedException("move not implemented...");
                }
            }
            return (isNextMoveValid, guardInfo);
        }

        /// <summary>
        /// returns new direction + if a move with this direction would be valid; or if the guard would walk outside the grid if moved
        /// </summary>
        /// <param name="input"></param>
        /// <param name="guardInfo"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        private (GuardDirection, bool) getNewGuardDirection(List<List<char>> input, GuardInfo guardInfo)
        {
            var turnCount = 0;
            var direction = guardInfo.Direction;
            while (turnCount < 4)
            {
                switch (direction)
                {
                    case GuardDirection.Up:
                        if (guardInfo.PosY == 0)
                        {
                            return (direction, false);
                        }
                        if (input[guardInfo.PosY - 1][guardInfo.PosX] == '#')
                        {
                            direction = GuardDirection.Right;
                        }
                        else
                        {
                            return (direction, true);
                        }
                        break;
                    case GuardDirection.Right:
                        if (guardInfo.PosX == input[guardInfo.PosY].Count - 1)
                        {
                            return (direction, false);
                        }
                        if (input[guardInfo.PosY][guardInfo.PosX + 1] == '#')
                        {
                            direction = GuardDirection.Down;
                        }
                        else
                        {
                            return (direction, true);
                        }
                        break;
                    case GuardDirection.Left:
                        if (guardInfo.PosX == 0)
                        {
                            return (direction, false);
                        }
                        if (input[guardInfo.PosY][guardInfo.PosX - 1] == '#')
                        {
                            direction = GuardDirection.Up;
                        }
                        else
                        {
                            return (direction, true);
                        }
                        break;
                    case GuardDirection.Down:
                        if (guardInfo.PosY == input.Count - 1)
                        {
                            return (direction, false);
                        }
                        if (input[guardInfo.PosY + 1][guardInfo.PosX] == '#')
                        {
                            direction = GuardDirection.Left;
                        }
                        else
                        {
                            return (direction, true);
                        }
                        break;
                    default:
                        throw new NotImplementedException("move not implemented...");
                }
                turnCount++;
            }
            throw new Exception("max turn count reached...");
        }

        private GuardInfo getGuardInfo(List<List<char>> input)
        {
            for (var iRow = 0; iRow < input.Count; iRow++)
            {
                for (var iCol = 0; iCol < input[iRow].Count; iCol++)
                {
                    var cell = input[iRow][iCol];
                    if ("^><v".Contains(cell))
                    {
                        var guardDirection = GuardDirection.Up;
                        switch (cell)
                        {
                            case '^':
                                guardDirection = GuardDirection.Up;
                                break;
                            case '>':
                                guardDirection = GuardDirection.Right;
                                break;
                            case '<':
                                guardDirection = GuardDirection.Left;
                                break;
                            case 'v':
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

        public override string ToString()
        {
            return $"{ PosX };{ PosY };{ Direction }";
        }
    }

    public enum GuardDirection
    {
        Up, Down, Left, Right
    }
}