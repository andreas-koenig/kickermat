namespace Communication.NetworkLayer.Packets.Udp
{
    using System;
    using Enums;
    using GameProperties;

    /// <summary>
    /// The networkobject containing the new player positions.
    /// </summary>
    public class PlayerPosition
    {
        public UdpPacketType PacketType { get; set; }

        /// <summary>
        /// Length of the UDP-datagram
        /// </summary>
        public const int datagramLength = 24;

        /// <summary>
        /// UDP-Datagram
        /// </summary>
        private byte[] positionPayload;

        /// <summary>
        /// The current keeper position.
        /// </summary>
        private ushort keeperPosition;

        /// <summary>
        /// The current keeper angle.
        /// </summary>
        private short keeperAngle;

        /// <summary>
        /// The current defense position.
        /// </summary>
        private ushort defensePosition;

        /// <summary>
        /// The current defense angle.
        /// </summary>
        private short defenseAngle;

        /// <summary>
        /// The current midfield position.
        /// </summary>
        private ushort midfieldPosition;

        /// <summary>
        /// The current midfield angle.
        /// </summary>
        private short midfieldAngle;

        /// <summary>
        /// The current striker position.
        /// </summary>
        private ushort strikerPosition;

        /// <summary>
        /// The current striker angle.
        /// </summary>
        private short strikerAngle;

        /// <summary>
        /// Gets or sets the keeper position.
        /// </summary>
        public ushort KeeperPosition
        {
            get
            {
                return this.keeperPosition;
            }

            set
            {
                this.keeperPosition = value;
                this.OptionsValidFor |= PositionBits.KeeperPosition;
            }
        }

        /// <summary>
        /// Gets or sets the keeper angel.
        /// </summary>
        public short KeeperAngle
        {
            get
            {
                return this.keeperAngle;
            }

            set
            {
                this.keeperAngle = value;
                this.OptionsValidFor |= PositionBits.KeeperAngle;
            }
        }

        /// <summary>
        /// Gets or sets the defense position.
        /// </summary>
        public ushort DefensePosition
        {
            get
            {
                return this.defensePosition;
            }

            set
            {
                this.defensePosition = value;
                this.OptionsValidFor |= PositionBits.DefensePosition;
            }
        }

        /// <summary>
        /// Gets or sets the keeper angel.
        /// </summary>
        public short DefenseAngle
        {
            get
            {
                return this.defenseAngle;
            }

            set
            {
                this.defenseAngle = value;
                this.OptionsValidFor |= PositionBits.DefenseAngle;
            }
        }

        /// <summary>
        /// Gets or sets the midfield position.
        /// </summary>
        public ushort MidfieldPosition
        {
            get
            {
                return this.midfieldPosition;
            }

            set
            {
                this.midfieldPosition = value;
                this.OptionsValidFor |= PositionBits.MidfieldPosition;
            }
        }

        /// <summary>
        /// Gets or sets the keeper angel.
        /// </summary>
        public short MidfieldAngle
        {
            get
            {
                return this.midfieldAngle;
            }

            set
            {
                this.midfieldAngle = value;
                this.OptionsValidFor |= PositionBits.MidfieldAngle;
            }
        }

        /// <summary>
        /// Gets or sets the striker position.
        /// </summary>
        public ushort StrikerPosition
        {
            get
            {
                return this.strikerPosition;
            }

            set
            {
                this.strikerPosition = value;
                this.OptionsValidFor |= PositionBits.StrikerPosition;
            }
        }

        /// <summary>
        /// Gets or sets the keeper angle.
        /// </summary>
        public short StrikerAngle
        {
            get
            {
                return this.strikerAngle;
            }

            set
            {
                this.strikerAngle = value;
                this.OptionsValidFor |= PositionBits.StrikerAngle;
            }
        }

        /// <summary>
        /// Gets or sets the sequence number.
        /// </summary>
        public static uint SequenceNumber { get; set; }

        //TODO: Is "All" needed? Otherwise use PlayerType instead of Bar?
        public PlayerPosition(Bar bar, ushort position, short angle, UdpPacketType packetType = UdpPacketType.SetPositionsAndAngles, bool waitForResponse = false)
        {
            positionPayload = new byte[datagramLength];

            switch (bar.barSelection)
            {
                case BarType.All:

                    this.KeeperPosition = position;
                    this.DefensePosition = position;
                    this.MidfieldPosition = position;
                    this.StrikerPosition = position;

                    this.KeeperAngle = angle;
                    this.DefenseAngle = angle;
                    this.MidfieldAngle = angle;
                    this.StrikerAngle = angle;

                    if (waitForResponse)
                    {
                        this.ReplyRequested = PositionBits.All;
                    }

                    break;
                case BarType.Keeper:

                    this.KeeperPosition = position;
                    this.KeeperAngle = angle;

                    if (waitForResponse)
                    {
                        this.ReplyRequested |= PositionBits.KeeperPosition;
                        this.ReplyRequested |= PositionBits.KeeperAngle;
                    }

                    break;
                case BarType.Defense:
                    this.DefensePosition = position;
                    this.DefenseAngle = angle;

                    if (waitForResponse)
                    {
                        this.ReplyRequested |= PositionBits.DefensePosition;
                        this.ReplyRequested |= PositionBits.DefenseAngle;
                    }

                    break;
                case BarType.Midfield:
                    this.MidfieldPosition = position;
                    this.MidfieldAngle = angle;

                    if (waitForResponse)
                    {
                        this.ReplyRequested |= PositionBits.MidfieldPosition;
                        this.ReplyRequested |= PositionBits.MidfieldAngle;
                    }

                    break;
                case BarType.Striker:
                    this.StrikerPosition = position;

                    if (waitForResponse)
                    {
                        this.ReplyRequested |= PositionBits.StrikerPosition;
                    }
                    break;
                default:
                    throw new ArgumentException("playerBar");
            }
        }

        public PlayerPosition(Bar bar, UdpPacketType packetType)
        {
            positionPayload = new byte[datagramLength];

            //TODO: Move all bars ?! if-clause from legacy-code
            if (!bar.barSelection.Equals(BarType.All))
            {
                if (packetType.Equals(UdpPacketType.SetMinPosition))
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)packetType), 0, this.positionPayload, 0, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)bar.barSelection), 0, this.positionPayload, 2, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)0), 0, this.positionPayload, 4, 2);
                    this.ZeroFillDatagramFromOffset(6);
                } 
                else if (packetType.Equals(UdpPacketType.SetMaxPosition))
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)packetType), 0, this.positionPayload, 0, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)bar.barSelection), 0, this.positionPayload, 2, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)255), 0, this.positionPayload, 4, 2);
                    this.ZeroFillDatagramFromOffset(6);
                }
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
            this.ReplyRequested = 0x00;
        }

        /// <summary>
        /// Serializes the object to a byte[] for sending it on the network layer.
        /// </summary>
        /// <returns>Returns the current instance represented as a byte[].</returns>
        public byte[] Serialize()
        {
            byte[] datagram = new byte[24];

            Buffer.BlockCopy(BitConverter.GetBytes((ushort)UdpPacketType.SetPositionsAndAngles), 0, datagram, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.keeperPosition), 0, datagram, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.keeperAngle), 0, datagram, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.defensePosition), 0, datagram, 6, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.defenseAngle), 0, datagram, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.midfieldPosition), 0, datagram, 10, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.midfieldAngle), 0, datagram, 12, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.strikerPosition), 0, datagram, 14, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.strikerAngle), 0, datagram, 16, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.SequenceNumber), 0, datagram, 18, 4);
            datagram[22] = (byte)this.OptionsValidFor;
            datagram[23] = (byte)this.ReplyRequested;

            return datagram;
        }

        /// <summary>
        /// Fills the datadatagram with zeroes from the offset to the end.
        /// </summary>
        /// <param name="offset">The offset.</param>
        private void ZeroFillDatagramFromOffset(int offset)
        {
            if (offset > this.positionPayload.Length)
            {
                throw new ArgumentException("Offset is out of datagram bounds");
            }

            for (int i = offset; i < this.positionPayload.Length; i++)
            {
                this.positionPayload[i] = 0x00;
            }
        }
    }
}
