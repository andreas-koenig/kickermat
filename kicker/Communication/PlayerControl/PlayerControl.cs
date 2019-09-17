﻿namespace Communication.PlayerControl.UdpGateway
{
    using System;
    using System.Threading;
    using GameProperties;
    using NetworkLayer.Packets.Udp;
    using NetworkLayer.Packets.Udp.Enums;
    using NetworkLayer.Udp;
    /// <summary>
    /// Class for controlling the players via an IP-CAN-Gateway.
    /// </summary>
 
    public class PlayerControl : IPlayerControl, IDisposable
    {
        /// <summary>
        /// The netwokr layer instance which is used for sending/receiving packages.
        /// </summary>
        private readonly NetworkLayer networkLayer;

        /// <summary>
        /// Lock object for <see cref="networkObject"/>
        /// </summary>
        private readonly object lockerNetworkObject;

        /// <summary>
        /// Backing storage for <see cref="NetworkObject"/>
        /// </summary>
        private PlayerPositions networkObject;

        /// <summary>
        /// Reset event for starting/stopping cyclic transmission.
        /// </summary>
        private ManualResetEvent resetEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerControl"/> class.
        /// </summary>
        public PlayerControl()
        {
            if (this.networkLayer == null)
            {
                //SwissKnife.ShowError(this, "No network service available.");
            }

            this.lockerNetworkObject = new object();
            this.NetworkObject = new PlayerPositions();

            this.resetEvent = new ManualResetEvent(false);

            //Thread senderThread = new Thread(this.ContiniousSending);
            //senderThread.Name = "Sending package to controller";
            //senderThread.IsBackground = true;

            //this.MessageIntervall = 5;
            //senderThread.Start();
        }

        /// <summary>
        /// Gets or sets the intervall to wait between sending messages.
        /// </summary>
        public int MessageIntervall { get; set; }

        /// <summary>
        /// Gets or sets the network object.
        /// </summary>
        /// <value>The network object.</value>
        private PlayerPositions NetworkObject
        {
            get
            {
                lock (this.lockerNetworkObject)
                {
                    return this.networkObject;
                }
            }

            set
            {
                lock (this.lockerNetworkObject)
                {
                    this.networkObject = value;
                }
            }
        }

        /// <summary>
        /// Moves a player.
        /// </summary>
        /// <param name="playerBar">The player bar.</param>
        /// <param name="newPlayerPosition">The new player position.</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        public void MovePlayer(Bar playerBar, ushort newPlayerPosition, bool waitForResponse)
        {
            switch (playerBar.barSelection)
            {
                case BarType.All:
                    this.NetworkObject.KeeperPosition = newPlayerPosition;
                    this.NetworkObject.DefensePosition = newPlayerPosition;
                    this.NetworkObject.MidfieldPosition = newPlayerPosition;
                    this.NetworkObject.StrikerPosition = newPlayerPosition;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested = PositionBits.All;
                    }

                    break;
                case BarType.Keeper:
                    this.NetworkObject.KeeperPosition = newPlayerPosition;
                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.KeeperPosition;
                    }

                    break;
                case BarType.Defense:
                    this.NetworkObject.DefensePosition = newPlayerPosition;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.DefensePosition;
                    }

                    break;
                case BarType.Midfield:
                    this.NetworkObject.MidfieldPosition = newPlayerPosition;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.MidfieldPosition;
                    }

                    break;
                case BarType.Striker:
                    this.NetworkObject.StrikerPosition = newPlayerPosition;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.StrikerPosition;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("playerBar");
            }
            this.networkLayer.udpConnection.Send(this.NetworkObject);
            if (waitForResponse)
            {
                // TODO: Check return code in Packet
                this.networkLayer.udpConnection.Read();
            }
        }

        /// <summary>
        /// Sets the angle of a bar.
        /// </summary>
        /// <param name="bar">The bar which will be rotated.</param>
        /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
        public void SetAngle(Bar bar, short angle)
        {
            this.SetAngle(bar, angle, false);
        }

        /// <summary>
        /// Sets the angle of a bar.
        /// </summary>
        /// <param name="bar">The bar which will be rotated.</param>
        /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        public void SetAngle(Bar bar, short angle, bool waitForResponse)
        {
            switch (bar.barSelection)
            {
                case BarType.All:
                    this.NetworkObject.KeeperAngle = angle;
                    this.NetworkObject.DefenseAngel = angle;
                    this.NetworkObject.MidfieldAngel = angle;
                    this.NetworkObject.StrikerAngel = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested = PositionBits.All;
                    }

                    break;
                case BarType.Keeper:
                    this.NetworkObject.KeeperAngle = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.KeeperAngle;
                    }

                    break;
                case BarType.Defense:
                    this.NetworkObject.DefenseAngel = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.DefenseAngle;
                    }

                    break;
                case BarType.Midfield:
                    this.NetworkObject.MidfieldAngel = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.MidfieldAngle;
                    }

                    break;
                case BarType.Striker:
                    this.NetworkObject.StrikerAngel = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.StrikerAngle;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("bar");
            }
            this.networkLayer.udpConnection.Send(this.NetworkObject);
            if (waitForResponse)
            {
                // TODO: Check return code in Packet
                this.networkLayer.udpConnection.Read();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.resetEvent != null)
            {
                this.resetEvent.Close();
                this.resetEvent = null;
            }
        }

        /// <summary>
        /// Method with loop for continious sending messages for positioning.
        /// </summary>
        private void ContiniousSending()
        {
            //Unused
            //while (true)
            //{
            //    this.resetEvent.WaitOne();

            //    this.networkLayer.Send(this.NetworkObject);
            //    this.NetworkObject.ClearReplyRequested();

            //    Thread.Sleep(this.MessageIntervall);
            //}
        }

        public void SetPlayerAnglePass(Bar bar, bool wait = false)
        {
            throw new NotImplementedException();
        }

        public void SetPlayerAngleBlock(Bar bar, bool wait = false)
        {
            throw new NotImplementedException();
        }
    }
}
