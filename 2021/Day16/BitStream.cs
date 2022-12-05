using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class BitStream : IDisposable
    {
        private readonly Stream _stream;
        private int _currentBytePosition = 0;
        private byte? _currentByte;

        public long Position => _stream.Position * 8 + _currentBytePosition;
        public long Length => _stream.Length * 8;


        public BitStream(Stream stream)
        {
            _stream = stream;
        }

        public int ReadBit()
        {
            if (_currentBytePosition > 7 || _currentByte is null)
            {
                int newByte = _stream.ReadByte();
                if (newByte >= 0)
                {
                    _currentByte = (byte)newByte;
                    _currentBytePosition = 0;
                }
                else  // Reached end of stream
                {
                    return -1;
                }
            }

            int bit = (_currentByte.Value >> (7 - _currentBytePosition)) & 1;
            _currentBytePosition++;
            bool value = true;

            return bit;
        }

        public IEnumerable<int> ReadBits(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return ReadBit();
            }
        }

        public void Dispose() =>_stream.Dispose();
    }
}
