var input = File.ReadAllLines("input");
var exampleInput = """
                   5764801
                   17807724
                   """.Split(Environment.NewLine);
                   
int cardPublicKey = int.Parse(input[0]);
int doorPublicKey = int.Parse(input[1]);

var results = CalculateLoopSizes(cardPublicKey, doorPublicKey, 7);
int cardLoopSize = results[cardPublicKey];

long encryptionKey = TransformForLoopSize(doorPublicKey, cardLoopSize);

Console.WriteLine($"Encryption key: {encryptionKey}");

return;


static Dictionary<int,int> CalculateLoopSizes(int publicKey1, int publicKey2, int subjectNumber)
{
    var loopSizes = new Dictionary<int, int>();

    long value = 1;
    int loops = 0;
    while (loopSizes.Count < 2)
    {
        value = (value * subjectNumber) % 20201227;
        loops++;

        if (value == publicKey1)
            loopSizes[publicKey1] = loops;
        else if (value == publicKey2)
            loopSizes[publicKey2] = loops;
    }

    return loopSizes;
}

static long TransformForLoopSize(int subjectNumber, int loopSize)
{
    long value = 1;
    for (int i = 0; i < loopSize; i++)
    {
        value = value * subjectNumber % 20201227;
    }

    return value;
}