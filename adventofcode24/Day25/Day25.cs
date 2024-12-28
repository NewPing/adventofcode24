using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adventofcode24.Day25
{
    public class Day25
    {
        public Day25()
        {
            // Read example input from file
            var inputLines = File.ReadAllLines(@"day25\input2.txt").ToList();
            var keysAndLocks = getKeysAndLocks(inputLines);

            var matchCount = 0;
            foreach (var lockItem in keysAndLocks.Where(x => x.Type == AType.Lock))
            {
                foreach (var key in keysAndLocks.Where(x => x.Type == AType.Key))
                {
                    if (isMatch(lockItem, key))
                    {
                        matchCount++;
                    }
                }
            }

            Console.WriteLine(matchCount);
        }

        public bool isMatch(Struktur _lock, Struktur _key)
        {
            for (int i = 0; i < _key.Numbers.Count; i++)
            {
                if (_key.Numbers[i] + _lock.Numbers[i] > _key.Numbers.Count)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Struktur> getKeysAndLocks(List<string> inputLines)
        {
            var structures = new List<Struktur>();
            var rowsBefore = new List<string>();
            foreach (var row in inputLines)
            {
                if (string.IsNullOrWhiteSpace(row))
                {
                    structures.Add(new Struktur(rowsBefore));
                    rowsBefore = new List<string>();
                } else
                {
                    rowsBefore.Add(row);
                }
            }
            // add last structure which didnt get captured because we got no empty line at the end of the input.
            structures.Add(new Struktur(rowsBefore));

            return structures;
        }

    }

    public class Struktur
    {
        public AType Type { get; set; } = AType.Key;
        public List<string> Content { get; set; } = new List<string>();
        public List<int> Numbers { get; set; } = new List<int>();

        public Struktur(List<string> _content)
        {
            Content = _content;

            //set lock or key
            if (Content[0].All(c => c == '#'))
            {
                Type = AType.Lock;
            }
            else
            {
                Type = AType.Key;
            }

            
            if(Type == AType.Key)
            {
                Content.Reverse();
            }

            //count numbers
            for (int iCol = 0; iCol < Content[iCol].Length; iCol++)
            {
                int number = 0;
                for (int iRow = 1; iRow < Content.Count; iRow++)
                {
                    if(Content[iRow][iCol] == '#')
                    {
                        number++;
                    }
                    else
                    {
                        break;
                    }
                }
                Numbers.Add(number);
            }
        }

    }

    public enum AType
    {
        Key,

        Lock
    }
}
