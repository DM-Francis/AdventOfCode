using Xunit;

namespace Day16.Test
{
    public class PacketTests
    {
        [Fact]
        public void LiteralValuePacketContructorWorksCorrectly()
        {
            // Arrange
            byte[] literalData = new byte[] { 0b_10111111, 0b_10001010 };
            int version = 6;

            // Act
            var packet = new LiteralValuePacket(version, literalData);

            // Assert
            Assert.Equal(version, packet.Version);
            Assert.Equal(2021, packet.LiteralValue);
        }

        [Fact]
        public void LiteralValuePacketContructorWorksCorrectlyWithPaddedZerosAtStart()
        {
            // Arrange
            byte[] literalData = new byte[] { 0b_0000001, 0b_01111111, 0b_00010100 };
            int version = 6;

            // Act
            var packet = new LiteralValuePacket(version, literalData);

            // Assert
            Assert.Equal(version, packet.Version);
            Assert.Equal(2021, packet.LiteralValue);
        }
    }
}