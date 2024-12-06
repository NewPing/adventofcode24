using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode24.Day05
{
    internal class Day05
    {
        public Day05()
        {
            var input = File.ReadAllLines(@"day05\input1.txt").ToList();

            var pageOrderingRules = new List<List<int>>();
            var updatePageNumbers = new List<List<int>>();
            var isFirstInputSection = true;
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    isFirstInputSection = false;
                } else
                {
                    if (isFirstInputSection)
                    {
                        pageOrderingRules.Add(Regex.Matches(line, @"\d+").Select(y => int.Parse(y.Value)).ToList());
                    } else
                    {
                        updatePageNumbers.Add(Regex.Matches(line, @"\d+").Select(y => int.Parse(y.Value)).ToList());
                    }
                }
            }

            Console.WriteLine(part1(pageOrderingRules, updatePageNumbers));
            Console.WriteLine(part2(pageOrderingRules, updatePageNumbers));
        }

        private int part1(List<List<int>> pageOrderingRules, List<List<int>> updatePageNumbers)
        {
            var goodUpdatePageNumbers = new List<List<int>>();
            foreach (var num in updatePageNumbers)
            {
                var ruleError = false;
                foreach (var rule in pageOrderingRules)
                {
                    if (num.Contains(rule.First()) && num.Contains(rule.Last()))
                    {
                        var iNum1 = num.FindIndex(x => x == rule.First());
                        var iNum2 = num.FindIndex(x => x == rule.Last());
                        if (iNum1 > iNum2)
                        {
                            ruleError = true;
                        }
                    }
                }
                if (!ruleError)
                {
                    goodUpdatePageNumbers.Add(num);
                }
            }

            var result = 0;
            foreach(var num in goodUpdatePageNumbers)
            {
                result += num[num.Count() / 2];
            }

            return result;
        }

        private int part2(List<List<int>> pageOrderingRules, List<List<int>> updatePageNumbers)
        {
            var usedToBeBadUpdatePageNumbers = new List<List<int>>();
            foreach (var num in updatePageNumbers)
            {
                var hadIssues = false;
                while (!isCorrectlyOrdered(pageOrderingRules, num))
                {
                    hadIssues = true;
                    foreach (var rule in pageOrderingRules)
                    {
                        if (num.Contains(rule.First()) && num.Contains(rule.Last()))
                        {
                            var iNum1 = num.FindIndex(x => x == rule.First());
                            var iNum2 = num.FindIndex(x => x == rule.Last());
                            if (iNum1 > iNum2)
                            {
                                var tmp = num[iNum1];
                                num[iNum1] = num[iNum2];
                                num[iNum2] = tmp;
                            }
                        }
                    }
                }

                if (hadIssues)
                {
                    usedToBeBadUpdatePageNumbers.Add(num);
                }
            }

            var result = 0;
            foreach (var num in usedToBeBadUpdatePageNumbers)
            {
                result += num[num.Count() / 2];
            }

            return result;
        }

        private bool isCorrectlyOrdered(List<List<int>> pageOrderingRules, List<int> updatePageNumber)
        {
            foreach (var rule in pageOrderingRules)
            {
                if (updatePageNumber.Contains(rule.First()) && updatePageNumber.Contains(rule.Last()))
                {
                    var iNum1 = updatePageNumber.FindIndex(x => x == rule.First());
                    var iNum2 = updatePageNumber.FindIndex(x => x == rule.Last());
                    if (iNum1 > iNum2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
