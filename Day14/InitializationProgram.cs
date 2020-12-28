using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    public class InitializationProgram
    {
        private readonly string[] _program;

        public Dictionary<long, long> Memory { get; } = new();

        public long Current1Mask { get; private set; }
        public long Current0Mask { get; private set; }

        public string CurrentV2Mask { get; private set; }

        public InitializationProgram(string[] rawProgram)
        {
            _program = rawProgram;
        }

        public void Run()
        {
            foreach(string command in _program)
            {
                ProcessCommand(command);
            }
        }

        public void RunV2()
        {
            foreach (string command in _program)
            {
                ProcessCommandV2(command);
            }
        }

        private void ProcessCommand(string command)
        {
            string[] commandSplit = command.Split(" = ");

            if (commandSplit[0] == "mask")
            {
                string mask1 = commandSplit[1].Replace('X', '0').PadLeft(36, '0');
                string mask0 = commandSplit[1].Replace('X', '1').PadLeft(36, '1');

                Current1Mask = Convert.ToInt64(mask1, 2);
                Current0Mask = Convert.ToInt64(mask0, 2);
            }
            else if (commandSplit[0].Contains("mem"))
            {
                string[] commandTypeSplit = commandSplit[0].Split('[', ']');
                long address = long.Parse(commandTypeSplit[1]);
                long value = long.Parse(commandSplit[1]);

                long finalValue = (value | Current1Mask) & Current0Mask;

                Memory[address] = finalValue;
            }
        }

        private void ProcessCommandV2(string command)
        {
            string[] commandSplit = command.Split(" = ");

            if (commandSplit[0] == "mask")
            {
                CurrentV2Mask = commandSplit[1];
            }
            else if (commandSplit[0].Contains("mem"))
            {
                string[] commandTypeSplit = commandSplit[0].Split('[', ']');
                long initialAddress = long.Parse(commandTypeSplit[1]);
                long value = long.Parse(commandSplit[1]);

                string addressBinary = Convert.ToString((long)initialAddress, 2).PadLeft(36, '0');

                var addressTemplateBuilder = new StringBuilder();

                for (int i = 0; i < CurrentV2Mask.Length; i++)
                {
                    char nextVal = CurrentV2Mask[i] switch
                    {
                        '0' => addressBinary[i],
                        '1' => '1',
                        'X' => 'X',
                        _ => throw new InvalidOperationException("Unrecognised mask character")
                    };

                    addressTemplateBuilder.Append(nextVal);
                }

                string finalAddressTemplate = addressTemplateBuilder.ToString();

                int[] XPositions = finalAddressTemplate
                    .Select((value, index) => (value, index))
                    .Where(a => a.value == 'X')
                    .Select(a => a.index)
                    .ToArray();

                int possibleReplacements = (int)Math.Pow(2, XPositions.Length);
                var finalAddresses = new List<long>();

                for (int r = 0; r < possibleReplacements; r++)
                {
                    string replacementBinary = Convert.ToString(r, 2).PadLeft(XPositions.Length, '0');
                    char[] address = finalAddressTemplate.ToArray();

                    for (int i = 0; i < XPositions.Length; i++)
                    {
                        address[XPositions[i]] = replacementBinary[i];
                    }

                    long finalAddress = Convert.ToInt64(new string(address), 2);
                    finalAddresses.Add(finalAddress);
                }

                foreach(long address in finalAddresses)
                {
                    Memory[address] = value;
                }
            }
        }
    }
}
