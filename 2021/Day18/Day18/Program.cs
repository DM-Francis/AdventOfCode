using Day18;

var input = File.ReadAllLines("input.txt");
var exampleInput = """
                   [[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
                   [[[5,[2,8]],4],[5,[[9,9],0]]]
                   [6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
                   [[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
                   [[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
                   [[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
                   [[[[5,4],[7,7]],8],[[8,3],8]]
                   [[9,3],[[9,9],[6,[4,9]]]]
                   [[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
                   [[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]
                   """.Split(Environment.NewLine);

var snailfishNumbers = input.Select(SnailfishNumber.Parse).ToList();

var first = snailfishNumbers[0];
var sum = snailfishNumbers.Skip(1).Aggregate(first, (a,b) => a + b);

Console.WriteLine($"Magnitude of final sum: {sum.GetMagnitude()}");

int maxMagnitude = 0;
for (int a = 0; a < snailfishNumbers.Count; a++)
{
    for (int b = 0; b < snailfishNumbers.Count; b++)
    {
        if (a == b)
            continue;

        var numA = SnailfishNumber.Parse(input[a]);
        var numB = SnailfishNumber.Parse(input[b]);
        int magnitude = (numA + numB).GetMagnitude();
        if (magnitude > maxMagnitude)
            maxMagnitude = magnitude;
    }
}

Console.WriteLine($"Max magnitude from two numbers: {maxMagnitude}");