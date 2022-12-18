using System.Collections;

string dataHex = File.ReadAllLines("input.txt")[0];
string testData = "A0016C880162017C3686B18A3D4780";

var dataBytes = Convert.FromHexString(dataHex);
var result = ParsePacketTreeFromData(dataBytes);

// Part 1
int sumOfVersionNumbers = result.GetVersionNumberSum();
Console.WriteLine($"Sum of version numbers: {sumOfVersionNumbers}");

// Part 2
ulong evaluatedExpression = result.GetValue();
Console.WriteLine($"Evaluated expression: {evaluatedExpression}");

static Packet ParsePacketTreeFromData(byte[] data)
{
    var preparedBytes = data.Select(ReverseBitsInByte).ToArray();
    var bitArray = new BitArray(preparedBytes);

    int index = 0;
    return ParseNextPacket(bitArray, ref index);
}

static byte ReverseBitsInByte(byte value)
{
    int output = 0;
    output |= (value & 1) << 7;
    output |= (value & 2) << 5;
    output |= (value & 4) << 3;
    output |= (value & 8) << 1;
    output |= (value & 16) >> 1;
    output |= (value & 32) >> 3;
    output |= (value & 64) >> 5;
    output |= (value & 128) >> 7;

    return (byte)output;
}

static Packet ParseNextPacket(BitArray bitArray, ref int currentIndex)
{
    int version = (int)BitsToUInt64(bitArray, ref currentIndex, 3);
    int typeId = (int)BitsToUInt64(bitArray, ref currentIndex, 3);

    switch (typeId)
    {
        case 4:
            ulong value = GetLiteralValueFromBitArray(bitArray, ref currentIndex);
            return new LiteralPacket(version, value);
        default:
            int lengthTypeId = (int)BitsToUInt64(bitArray, ref currentIndex, 1);
            IEnumerable<Packet> subPackets;
            switch (lengthTypeId)
            {
                case 0:
                {
                    int length = (int)BitsToUInt64(bitArray, ref currentIndex, 15);
                    subPackets = ParsePacketsInNextNBits(bitArray, ref currentIndex, length);
                    break;
                }
                case 1:
                {
                    int length = (int)BitsToUInt64(bitArray, ref currentIndex, 11);
                    subPackets = ParseNextNPackets(bitArray, ref currentIndex, length);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(lengthTypeId), lengthTypeId, "Invalid length type id");
            };

            var packet = new OperatorPacket(version, typeId);
            packet.ChildPackets.AddRange(subPackets);
            return packet;
    }
}

static IEnumerable<Packet> ParsePacketsInNextNBits(BitArray bitArray, ref int currentIndex, long n)
{
    var packets = new List<Packet>();
    int startIndex = currentIndex;
    while (currentIndex < startIndex + n)
    {
        packets.Add(ParseNextPacket(bitArray, ref currentIndex));
    }

    if (currentIndex > startIndex + n)
        throw new InvalidOperationException("Used more bits than expected");

    return packets;
}

static IEnumerable<Packet> ParseNextNPackets(BitArray bitArray, ref int currentIndex, long n)
{
    var packets = new List<Packet>();
    for (int i = 0; i < n; i++)
    {
        packets.Add(ParseNextPacket(bitArray, ref currentIndex));
    }

    return packets;
}

static ulong GetLiteralValueFromBitArray(BitArray bitArray, ref int currentIndex)
{
    bool keepParsingGroups = true;
    var outputBits = new List<bool>();
    while (keepParsingGroups)
    {
        keepParsingGroups = bitArray[currentIndex++];
        for (int i = 0; i < 4; i++)
        {
            outputBits.Add(bitArray[currentIndex++]);
        }
    }

    var outputBitArray = new BitArray(outputBits.ToArray());
    int tempIndex = 0;
    return BitsToUInt64(outputBitArray, ref tempIndex, outputBits.Count);
}

static ulong BitsToUInt64(BitArray bitArray, ref int currentIndex, int length)
{
    if (length > 64)
        throw new ArgumentOutOfRangeException(nameof(length), length, "Length must be less than or equal to 32");
    
    ulong output = 0;
    for (int i = 0; i < length; i++)
    {
        ulong bit = bitArray[currentIndex + i] ? (ulong)1 : 0;
        output |= bit << (length - i - 1);
    }

    currentIndex += length;
    return output;
}

public abstract class Packet
{
    public int Version { get; protected set; }
    public abstract int TypeId { get; }

    public abstract int GetVersionNumberSum();
    public abstract ulong GetValue();
}

public class LiteralPacket : Packet
{
    public override int TypeId => 4;
    public ulong Value { get; }

    public LiteralPacket(int version, ulong value)
    {
        Version = version;
        Value = value;
    }

    public override int GetVersionNumberSum() => Version;
    public override ulong GetValue() => Value;
}

public class OperatorPacket : Packet
{
    public override int TypeId { get; }
    public List<Packet> ChildPackets { get; } = new();

    public OperatorPacket(int version, int typeId)
    {
        Version = version;
        TypeId = typeId;
    }
    
    public override int GetVersionNumberSum()
    {
        return Version + ChildPackets.Sum(c => c.GetVersionNumberSum());
    }

    public override ulong GetValue()
    {
        return TypeId switch
        {
            0 => ChildPackets.Aggregate((ulong)0, (agg, p) => agg + p.GetValue()),
            1 => ChildPackets.Aggregate((ulong)1, (agg, p) => agg * p.GetValue()),
            2 => ChildPackets.Aggregate(ulong.MaxValue, (agg, p) => Math.Min(agg, p.GetValue())),
            3 => ChildPackets.Aggregate((ulong)0, (agg, p) => Math.Max(agg, p.GetValue())),
            5 => ChildPackets[0].GetValue() > ChildPackets[1].GetValue() ? (ulong)1 : 0,
            6 => ChildPackets[0].GetValue() < ChildPackets[1].GetValue() ? (ulong)1 : 0,
            7 => ChildPackets[0].GetValue() == ChildPackets[1].GetValue() ? (ulong)1 : 0,
            _ => throw new ArgumentOutOfRangeException(nameof(TypeId), TypeId,
                "Invalid TypeId value for operator packet")
        };
    }
}