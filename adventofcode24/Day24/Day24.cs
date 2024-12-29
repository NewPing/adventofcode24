using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode24.Day24
{
    public class Day24
    {
        public Day24()
        {
            var inputList = File.ReadAllLines(@"day24\input2.txt").ToList();

            var p1Output = part1(inputList);
            Console.WriteLine(p1Output);
            Console.WriteLine(Convert.ToInt64(p1Output, 2));

        }

        public string part1(List<string> inputList)
        {
            int splitIndex = inputList.FindIndex(string.IsNullOrWhiteSpace);
            var inputSignals = inputList.GetRange(0, splitIndex);
            var inputGates = inputList.GetRange(splitIndex + 1, inputList.Count - splitIndex - 1);
            var inputGatesG = readInputSignals(inputSignals);
            var gates = readGates(inputGates, inputGatesG);
            var outputGates = gates.Where(x => x.Key.StartsWith("z")).OrderByDescending(x => x.Key).ToList();
            var outputString = string.Join("", outputGates.Select(x => x.GetValue() == true ? "1" : "0"));
            return outputString;
        }

        private List<Gate> readInputSignals(List<String> inputSignals)
        {
            var gates = new List<Gate>();
            foreach (var signal in inputSignals)
            {
                var parts = signal.Split(": ");
                gates.Add(new Gate()
                {
                    Key = parts[0],
                    Value = parts[1] == "1" ? true : false
                });
            }
            return gates;
        }

        private List<Gate> readGates(List<String> inputGates, List<Gate> inputSignals)
        {
            var isResolvementInProgress = true;
            var resolvedInputs = new List<string>();
            var gates = new List<Gate>(inputSignals);
            while (isResolvementInProgress)
            {
                isResolvementInProgress = false;
                foreach (var gateInput in inputGates)
                {
                    if (!resolvedInputs.Contains(gateInput))
                    {
                        isResolvementInProgress = true;

                        var parts = gateInput.Split(" ");
                        var input1 = parts[0];
                        var gateOperator = parts[1];
                        var input2 = parts[2];
                        var key = parts[4];

                        var inputGate1 = gates.Find(x => x.Key == input1);
                        var inputGate2 = gates.Find(x => x.Key == input2);

                        if (inputGate1 != null && inputGate2 != null)
                        {
                            gates.Add(new Gate()
                            {
                                Input1 = inputGate1,
                                Input2 = inputGate2,
                                GateOperator = (GateOperator)Enum.Parse(typeof(GateOperator), gateOperator),
                                Key = key,
                            });
                            resolvedInputs.Add(gateInput);
                        }
                    }
                }
            }
            
            return gates;
        }
    }
    
    public class Gate
    {
        public Gate Input1 { get; set; }
        public Gate Input2 { get; set; }
        public GateOperator GateOperator { get; set; }

        public string Key { get; set; }
        public bool? Value { get; set; }

        public bool GetValue()
        {
            if (Value is not null)
            {
                return Value.Value;
            } else
            {
                switch (GateOperator)
                {
                    case GateOperator.AND:
                        return Input1.GetValue() && Input2.GetValue();
                    case GateOperator.OR:
                        return Input1.GetValue() || Input2.GetValue();
                    case GateOperator.XOR:
                        return Input1.GetValue() ^ Input2.GetValue();
                    default:
                        throw new NotImplementedException("Case not implemented");
                }
            }
        }
    }

    public enum GateOperator
    {
        AND,
        OR,
        XOR
    }

}
