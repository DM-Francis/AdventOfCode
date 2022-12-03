using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public abstract class Packet
    {
        public int Version { get; }
        public abstract int TypeId { get; }

        public Packet(int version)
        {
            Version = version;
        }

        public static IEnumerable<Packet> FromRawData(byte[] rawData, int numPackets)
        {
            int version = rawData[0] >> 5;
            int typeId = (rawData[0] & 0b_00011100) >> 2;

            if (typeId == 4) // Literal packet
            {
                byte[] literalData = (byte[])rawData.Clone();
                literalData[0] &= 0b_00000011;  // Remove header bits
                yield return new LiteralValuePacket(version, literalData);
            }
            else // Operator packet
            {
                int lengthTypeId = (rawData[0] & 0b_00000010) >> 1;

                if (lengthTypeId == 0)
                {
                    int dataLengthBits = ((rawData[0] & 1) << 14) + (rawData[1] << 13) + ((rawData[2] & 0b11111100) >> 2);  // Next 15 bits

                    //yield return new OperationPacket(version, typeId, lengthTypeId, dataLengthBits, )
                }
                else if (lengthTypeId == 1)
                {
                    int dataLengthPackets = ((rawData[0] & 1) << 10) + (rawData[1] << 9) + ((rawData[2] & 0b11000000) >> 6); // Next 11 bits
                }

                throw new NotImplementedException();
            }
        }
    }

    public class OperationPacket : Packet
    {
        private readonly List<Packet> _subPackets = new();

        public override int TypeId { get; }
        public int LengthTypeId { get; }
        public int Length { get; }
        public IEnumerable<Packet> SubPackets => _subPackets.AsReadOnly();

        public OperationPacket(int version, int typeId, int lengthTypeId, int length, byte[] innerData) : base(version)
        {
            TypeId = typeId;
            LengthTypeId = lengthTypeId;
            Length = length;
        }
    }
}
