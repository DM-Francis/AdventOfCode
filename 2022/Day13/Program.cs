using System.Text;
using Day13;

var input = File.ReadAllText("input.txt");

// Part 1
var pairs = input.Split("\n\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

int indexSumOfCorrectlyOrderedPairs = 0;
for (var index = 1; index <= pairs.Length; index++)
{
    var pair = pairs[index - 1];
    var split = pair.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var first = ParsePacketFromInputLine(split[0]);
    var second = ParsePacketFromInputLine(split[1]);

    if (first.CompareTo(second) < 0)
        indexSumOfCorrectlyOrderedPairs += index;
}

Console.WriteLine($"Index sum of correctly ordered pairs: {indexSumOfCorrectlyOrderedPairs}");

// Part 2
var packetLines = input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
var packets = packetLines.Select(ParsePacketFromInputLine).ToList();

var dividers = new [] { ParsePacketFromInputLine("[[2]]"), ParsePacketFromInputLine("[[6]]") };

var allPackets = packets.Concat(dividers).ToList();
allPackets.Sort();

var index1 = allPackets.IndexOf(dividers[0]) + 1;
var index2 = allPackets.IndexOf(dividers[1]) + 1;
var decodeKey = index1 * index2;

Console.WriteLine($"Decode key: {decodeKey}");

static PacketList ParsePacketFromInputLine(string line)
{
    var tokens = SplitIntoTokens(line);
    var stack = new Stack<PacketList>();

    foreach (var token in tokens)
    {
        switch (token)
        {
            case "[":
                stack.Push(new PacketList());
                break;
            case "]":
                var finishedList = stack.Pop();
                if (stack.Count > 0)
                    stack.Peek().Items.Add(finishedList);
                else
                    return finishedList;
                break;
            case ",":
                break;
            default:
                int value = int.Parse(token);
                stack.Peek().Items.Add(new PacketInt(value));
                break;
        }
    }

    throw new InvalidOperationException("Parsing didn't end with a completed PacketList");
}

static IReadOnlyList<string> SplitIntoTokens(string line)
{
    var tokens = new List<string>();
    var current = new StringBuilder();
    foreach (char ch in line)
    {
        if (ch is '[' or ']' or ',')
        {
            if (current.Length > 0)
            {
                tokens.Add(current.ToString());
                current.Clear();
            }
            
            tokens.Add(ch.ToString());
        }
        else
        {
            current.Append(ch.ToString());
        }
    }

    return tokens;
}

