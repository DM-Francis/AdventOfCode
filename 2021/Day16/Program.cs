string dataHex = File.ReadAllLines("input.txt")[0];
string testData = "EE00D40C823060";


ReadData(Convert.FromHexString(testData));

static void ReadData(byte[] rawData)
{
    int version = rawData[0] >> 5;
    int typeId = (rawData[0] & 0b_00011100) >> 2;

    if (typeId == 4) // Literal packet
    {
        byte[] literalData = (byte[])rawData.Clone();
        literalData[0] &= 0b_00000011;  // Remove header bits
        ReadLiteralValue(literalData);
    }
    else // Operator packet
    {
        int lengthTypeId = (rawData[0] & 0b_00000010) >> 1;

        if (lengthTypeId == 0)
        {
            int dataLengthBits = ((rawData[0] & 1) << 14) + (rawData[1] << 13) + ((rawData[2] & 0b11111100) >> 2);  // Next 15 bits
        }
        else if (lengthTypeId == 1)
        {
            int dataLengthPackets = ((rawData[0] & 1) << 10) + (rawData[1] << 9) + ((rawData[2] & 0b11000000) >> 6); // Next 11 bits
        }
    }
}


static int ReadLiteralValue(byte[] literalData)
{
    bool readingDataBits = false;
    bool started = false;
    bool expectingMoreGroups = true;
    
    int literalValue = 0;
    int dataBitIndex = 0;
    int bitGroupIndex = 0;

    while (dataBitIndex < literalData.Length * 8)
    {
        int currentBit = literalData[dataBitIndex / 8] & (1 << (7 - dataBitIndex % 8));  // Extract only the bit for this loop - will either be 0 or a power of 2

        currentBit = Convert.ToInt32(currentBit > 0); // Now either 1 or 0

        if (!started)
        {
            if (currentBit == 1)
            {
                readingDataBits = true;
                started = true;
            }
        }
        else if (readingDataBits)
        {
            literalValue = (literalValue << 1) + currentBit;
            bitGroupIndex++;

            if (bitGroupIndex == 4)
            {
                if (!expectingMoreGroups)
                    break;

                bitGroupIndex = 0;
                readingDataBits = false;
            }
        }
        else
        {
            readingDataBits = true;

            if (currentBit == 0)
                expectingMoreGroups = false;
        }

        dataBitIndex++;
    }

    return literalValue;
}