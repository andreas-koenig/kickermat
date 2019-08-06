namespace Communication.NetworkLayer.Packets.Udp
{
    using System;
    using Enums;

    /// <summary>
    /// The networkobject containing the new player positions.
    /// </summary>
    public class PlayerPositions : NetworkObject
    {
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
        private short defenseAngel;

        /// <summary>
        /// The current midfield position.
        /// </summary>
        private ushort midfieldPosition;

        /// <summary>
        /// The current midfield angle.
        /// </summary>
        private short midfieldAngel;

        /// <summary>
        /// The current striker position.
        /// </summary>
        private ushort strikerPosition;

        /// <summary>
        /// The current striker angle.
        /// </summary>
        private short strikerAngel;
        
        /// <summary>
        /// Sequence number of the packet.
        /// </summary>
        private uint sequenceNumber;
        
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
        public short DefenseAngel
        {
            get
            {
                return this.defenseAngel;
            }

            set
            {
                this.defenseAngel = value;
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
        public short MidfieldAngel
        {
            get
            {
                return this.midfieldAngel;
            }

            set
            {
                this.midfieldAngel = value;
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
        /// Gets or sets the keeper angel.
        /// </summary>
        public short StrikerAngel
        {
            get
            {
                return this.strikerAngel;
            }

            set
            {
                this.strikerAngel = value;
                this.OptionsValidFor |= PositionBits.StrikerAngle;
            }
        }

        /// <summary>
        /// Gets or sets the sequence number.
        /// </summary>
        public uint SequenceNumber
        {
            get { return this.sequenceNumber; }
            set { this.sequenceNumber = value; }
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
            Buffer.BlockCopy(BitConverter.GetBytes(this.defenseAngel), 0, datagram, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.midfieldPosition), 0, datagram, 10, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.midfieldAngel), 0, datagram, 12, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.strikerPosition), 0, datagram, 14, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.strikerAngel), 0, datagram, 16, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.sequenceNumber), 0, datagram, 18, 4);
            datagram[22] = (byte)this.OptionsValidFor;
            datagram[23] = (byte)this.ReplyRequested;

            return datagram;
        }        
    }
}