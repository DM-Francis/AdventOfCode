var data = File.ReadAllLines("input.txt");
var testData = new string[]
{
    "00100",
    "11110",
    "10110",
    "10111",
    "10101",
    "01111",
    "00111",
    "11100",
    "10000",
    "11001",
    "00010",
    "01010"
};

//data = testData;


int totalLineCount = data.Length;
int[] bitCounts = GetBitCounts(data);

(int gammaRate, int epsilonRate) = GetGammaAndEpsilonRates(bitCounts, totalLineCount);

int oxygenRating = GetOxygenRating(data);
int co2ScrubberRating = GetCO2ScrubberRating(data);

Console.WriteLine($"Gamma rate: {gammaRate}");
Console.WriteLine($"Epsilon rate: {epsilonRate}");
Console.WriteLine($"Result: {gammaRate * epsilonRate}");
Console.WriteLine();
Console.WriteLine($"Oxygen rating: {oxygenRating}");
Console.WriteLine($"Scrubber rating: {co2ScrubberRating}");
Console.WriteLine($"Result: {oxygenRating * co2ScrubberRating}");


static int ConvertToDecimal(int[] bits)
{
    string bitString = string.Join("", bits);
    return Convert.ToInt32(bitString, 2);
}

static int[] GetBitCounts(string[] data)
{
    int[] bitCounts = new int[data[0].Length];

    foreach (string line in data)
    {
        for (int i = 0; i < line.Length; i++)
        {
            bitCounts[i] += int.Parse(line[i].ToString());
        }
    }

    return bitCounts;
}

static (int GammaRate, int EpsilonRate) GetGammaAndEpsilonRates(int[] bitCounts, int totalLineCount)
{
    int[] gammaRateBits = new int[bitCounts.Length];
    int[] epsilonRateBits = new int[bitCounts.Length];

    for (int i = 0; i < bitCounts.Length; i++)
    {
        if (bitCounts[i] > totalLineCount / 2.0)
        {
            gammaRateBits[i] = 1;
            epsilonRateBits[i] = 0;
        }
        else
        {
            gammaRateBits[i] = 0;
            epsilonRateBits[i] = 1;
        }
    }

    int gammaRate = ConvertToDecimal(gammaRateBits);
    int epsilonRate = ConvertToDecimal(epsilonRateBits);

    return (gammaRate, epsilonRate);
}

static int GetOxygenRating(string[] data) => GetRating(data, BitCriteria.MostCommon);
static int GetCO2ScrubberRating(string[] data) => GetRating(data, BitCriteria.LeastCommon);

static int GetRating(string[] data, BitCriteria bitCriteria)
{
    int bitIndex = 0;
    while (data.Length > 1)
    {
        data = FilterData(data, bitIndex, bitCriteria);
        bitIndex++;
    }

    return Convert.ToInt32(data[0], 2);
}

static string[] FilterData(string[] data, int bitIndex, BitCriteria bitCriteria)
{
    int totalCount = data.Length;
    int[] bitCounts = GetBitCounts(data);

    (char keyChar, char otherChar) = bitCriteria switch
    {
        BitCriteria.MostCommon => ('1', '0'),
        BitCriteria.LeastCommon => ('0', '1'),
        _ => throw new ArgumentOutOfRangeException(nameof(bitCriteria))
    };

    if (bitCounts[bitIndex] >= totalCount / 2.0)
    {
        return data.Where(s => s[bitIndex] == keyChar).ToArray();
    }
    else
    {
        return data.Where(s => s[bitIndex] == otherChar).ToArray();
    }
}

enum BitCriteria
{
    MostCommon,
    LeastCommon
}