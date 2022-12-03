namespace Day16
{
    public class LiteralValuePacket : Packet
    {
        public override int TypeId => 4;
        public int LiteralValue { get; }

        public LiteralValuePacket(int version, byte[] literalValueData) : base(version)
        {
            LiteralValue = ReadLiteralValue(literalValueData);
        }

        private static int ReadLiteralValue(byte[] literalValueData)
        {
            bool readingDataBits = false;
            bool started = false;
            bool expectingMoreGroups = true;

            int literalValue = 0;
            int dataBitIndex = 0;
            int bitGroupIndex = 0;

            while (dataBitIndex < literalValueData.Length * 8)
            {
                int currentBit = literalValueData[dataBitIndex / 8] & (1 << (7 - dataBitIndex % 8));  // Extract only the bit for this loop - will either be 0 or a power of 2

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
    }
}
