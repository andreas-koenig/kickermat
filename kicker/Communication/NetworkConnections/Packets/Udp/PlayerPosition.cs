using System;
using Communication.NetworkConnections.Packets.Udp.Enums;
using GameProperties;

namespace Communication.NetworkConnections.Packets.Udp
{
    /// <summary>
    /// The networkobject containing the new player positions.
    /// </summary>
    public class NetworkObject
    {
        /// <summary>
        /// Length of the UDP-datagram.
        /// </summary>
        public const int DatagramLength = 24;

        /// <summary>
        /// The current keeper position.
        /// </summary>
        private ushort _keeperPosition;

        /// <summary>
        /// The current keeper angle.
        /// </summary>
        private short _keeperAngle;

        /// <summary>
        /// The current defense position.
        /// </summary>
        private ushort _defensePosition;

        /// <summary>
        /// The current defense angle.
        /// </summary>
        private short _defenseAngle;

        /// <summary>
        /// The current midfield position.
        /// </summary>
        private ushort _midfieldPosition;

        /// <summary>
        /// The current midfield angle.
        /// </summary>
        private short _midfieldAngle;

        /// <summary>
        /// The current striker position.
        /// </summary>
        private ushort _strikerPosition;

        /// <summary>
        /// The current striker angle.
        /// </summary>
        private short _strikerAngle;

        // TODO: Use a Factory for similar Network-Objects ?
        public NetworkObject(Bar bar, ushort position, short angle, bool waitForResponse = false)
        {
            Datagram = new byte[DatagramLength];

            switch (bar?.BarSelection)
            {
                // TODO: Is "All" needed? Otherwise use PlayerType instead of Bar?
                case BarType.All:

                    KeeperPosition = position;
                    DefensePosition = position;
                    MidfieldPosition = position;
                    StrikerPosition = position;

                    KeeperAngle = angle;
                    DefenseAngle = angle;
                    MidfieldAngle = angle;
                    StrikerAngle = angle;

                    if (waitForResponse)
                    {
                        ReplyRequested = PositionBits.All;
                    }

                    break;
                case BarType.Keeper:

                    KeeperPosition = position;
                    KeeperAngle = angle;

                    if (waitForResponse)
                    {
                        ReplyRequested |= PositionBits.KeeperPosition;
                        ReplyRequested |= PositionBits.KeeperAngle;
                    }

                    break;
                case BarType.Defense:
                    DefensePosition = position;
                    DefenseAngle = angle;

                    if (waitForResponse)
                    {
                        ReplyRequested |= PositionBits.DefensePosition;
                        ReplyRequested |= PositionBits.DefenseAngle;
                    }

                    break;
                case BarType.Midfield:
                    MidfieldPosition = position;
                    MidfieldAngle = angle;

                    if (waitForResponse)
                    {
                        ReplyRequested |= PositionBits.MidfieldPosition;
                        ReplyRequested |= PositionBits.MidfieldAngle;
                    }

                    break;
                case BarType.Striker:
                    StrikerPosition = position;

                    if (waitForResponse)
                    {
                        ReplyRequested |= PositionBits.StrikerPosition;
                    }

                    break;
                default:
                    throw new ArgumentException("playerBar");
            }
        }

        public NetworkObject(Bar bar, UdpPacketType packetType)
        {
            Datagram = new byte[DatagramLength];

            // TODO: Move all bars ?! if-clause from legacy-code
            if (!bar.BarSelection.Equals(BarType.All))
            {
                if (packetType.Equals(UdpPacketType.SetMinPosition))
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)packetType), 0, Datagram, 0, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)bar.BarSelection), 0, Datagram, 2, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)0), 0, Datagram, 4, 2);
                    ZeroFillDatagramFromOffset(6);
                }
                else if (packetType.Equals(UdpPacketType.SetMaxPosition))
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)packetType), 0, Datagram, 0, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)bar.BarSelection), 0, Datagram, 2, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)255), 0, Datagram, 4, 2);
                    ZeroFillDatagramFromOffset(6);
                }
            }
        }

        public NetworkObject(Bar bar, ushort barLengthInPixel)
        {
            Datagram = new byte[DatagramLength];

            ushort udpId = (ushort)bar.BarSelection;
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.SetBarLengthInPixel), 0, Datagram, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(udpId), 0, Datagram, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(barLengthInPixel), 0, Datagram, 4, 2);
            ZeroFillDatagramFromOffset(6);
        }

        public NetworkObject(Bar bar, int nullAngle)
        {
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.SetNullAngle), 0, Datagram, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)bar.BarSelection), 0, Datagram, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((short)nullAngle), 0, Datagram, 0, 4);
            ZeroFillDatagramFromOffset(6);
        }

        public NetworkObject()
        {
            // NOTE: Reset positions with this object
            // NOTE: Integral values have default 0
            OptionsValidFor = PositionBits.All;
            ReplyRequested = PositionBits.None;
        }

        /// <summary>
        /// Gets or sets the sequence number.
        /// </summary>
        public uint SequenceNumber { get; set; }

        public UdpPacketType PacketType { get; set; }

        /// <summary>
        /// Gets the UDP-datagram.
        /// </summary>
        public byte[] Datagram { get; private set; }

        /// <summary>
        /// Gets or sets the keeper position.
        /// </summary>
        public ushort KeeperPosition
        {
            get
            {
                return _keeperPosition;
            }

            set
            {
                _keeperPosition = value;
                OptionsValidFor |= PositionBits.KeeperPosition;
            }
        }

        /// <summary>
        /// Gets or sets the keeper angel.
        /// </summary>
        public short KeeperAngle
        {
            get
            {
                return _keeperAngle;
            }

            set
            {
                _keeperAngle = value;
                OptionsValidFor |= PositionBits.KeeperAngle;
            }
        }

        /// <summary>
        /// Gets or sets the defense position.
        /// </summary>
        public ushort DefensePosition
        {
            get
            {
                return _defensePosition;
            }

            set
            {
                _defensePosition = value;
                OptionsValidFor |= PositionBits.DefensePosition;
            }
        }

        /// <summary>
        /// Gets or sets the keeper angel.
        /// </summary>
        public short DefenseAngle
        {
            get
            {
                return _defenseAngle;
            }

            set
            {
                _defenseAngle = value;
                OptionsValidFor |= PositionBits.DefenseAngle;
            }
        }

        /// <summary>
        /// Gets or sets the midfield position.
        /// </summary>
        public ushort MidfieldPosition
        {
            get
            {
                return _midfieldPosition;
            }

            set
            {
                _midfieldPosition = value;
                OptionsValidFor |= PositionBits.MidfieldPosition;
            }
        }

        /// <summary>
        /// Gets or sets the keeper angel.
        /// </summary>
        public short MidfieldAngle
        {
            get
            {
                return _midfieldAngle;
            }

            set
            {
                _midfieldAngle = value;
                OptionsValidFor |= PositionBits.MidfieldAngle;
            }
        }

        /// <summary>
        /// Gets or sets the striker position.
        /// </summary>
        public ushort StrikerPosition
        {
            get
            {
                return _strikerPosition;
            }

            set
            {
                _strikerPosition = value;
                OptionsValidFor |= PositionBits.StrikerPosition;
            }
        }

        /// <summary>
        /// Gets or sets the keeper angle.
        /// </summary>
        public short StrikerAngle
        {
            get
            {
                return _strikerAngle;
            }

            set
            {
                _strikerAngle = value;
                OptionsValidFor |= PositionBits.StrikerAngle;
            }
        }

        /// <summary>
        /// Gets or sets the valid options.
        /// </summary>
        public PositionBits OptionsValidFor { get; set; }

        /// <summary>
        /// Gets or sets whether a reply is wanted.
        /// </summary>
        public PositionBits ReplyRequested { get; set; }

        /// <summary>
        /// Clears the reply requested flags.
        /// </summary>
        public void ClearReplyRequested()
        {
            ReplyRequested = 0x00;
        }

        /// <summary>
        /// Serializes the object to a byte[] for sending it on the network layer.
        /// </summary>
        /// <returns>Returns the current instance represented as a byte[].</returns>
        public byte[] Serialize()
        {
            byte[] datagram = new byte[24];

            Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.SetPositionsAndAngles), 0, datagram, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(_keeperPosition), 0, datagram, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(_keeperAngle), 0, datagram, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(_defensePosition), 0, datagram, 6, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(_defenseAngle), 0, datagram, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(_midfieldPosition), 0, datagram, 10, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(_midfieldAngle), 0, datagram, 12, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(_strikerPosition), 0, datagram, 14, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(_strikerAngle), 0, datagram, 16, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(SequenceNumber), 0, datagram, 18, 4);
            datagram[22] = (byte)OptionsValidFor;
            datagram[23] = (byte)ReplyRequested;

            return datagram;
        }

        /// <summary>
        /// Fills the datadatagram with zeroes from the offset to the end.
        /// </summary>
        /// <param name="offset">The offset.</param>
        private void ZeroFillDatagramFromOffset(int offset)
        {
            if (offset > Datagram.Length)
            {
                throw new ArgumentException("Offset is out of datagram bounds");
            }

            for (int i = offset; i < Datagram.Length; i++)
            {
                Datagram[i] = 0x00;
            }
        }
    }
}
