using Day25;

var input = File.ReadAllLines("input.txt");
var exampleInput = """
                   1=-0-2
                   12111
                   2=0=
                   21
                   2=01
                   111
                   20012
                   112
                   1=-1=
                   1-12
                   12
                   1=
                   122
                   """.Split(Environment.NewLine);

var rawSnafus = input.Select(x => x.Trim()).ToList();
var snafus = rawSnafus.Select(Snafu.ToDecimal).ToList();
foreach(var snafu in snafus)
    Console.WriteLine(snafu);

long sum = snafus.Sum();
Console.WriteLine($"Sum in decimal: {sum}");

string sumInSnafu = Snafu.Sum(rawSnafus);
Console.WriteLine($"Sum in snafu: {sumInSnafu}");