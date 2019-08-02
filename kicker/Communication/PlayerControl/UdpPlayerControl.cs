namespace Communication.PlayerControl.UdpGateway
{
    using System;
    using System.Threading;
    using Game;
    using GlobalDataTypes;
    using NetworkLayer.Packets.Udp;
    using NetworkLayer.Packets.Udp.Enums;
    using NetworkLayer.Udp;
    using PluginSystem;
    using Utilities;

    /// <summary>
    /// Class for controlling the players via an IP-CAN-Gateway.
    /// </summary>
    public class UdpPlayerControl : IPlayerControl, IDisposable
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
        private PositionNetworkObject networkObject;

        /// <summary>
        /// Reset event for starting/stopping cyclic transmission.
        /// </summary>
        private ManualResetEvent resetEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpPlayerControl"/> class.
        /// </summary>
        public UdpPlayerControl()
        {
            Game.GameStarted += this.Game_GameStarted;
            Game.GameStopped += this.Game_GameStopped;

            this.networkLayer = ServiceLocator.LocateService<NetworkLayer>();
            if (this.networkLayer == null)
            {
                SwissKnife.ShowError(this, "No network service available.");
            }

            this.lockerNetworkObject = new object();
            this.NetworkObject = new PositionNetworkObject();

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
        private PositionNetworkObject NetworkObject
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
            switch (playerBar)
            {
                case Bar.All:
                    this.NetworkObject.KeeperPosition = newPlayerPosition;
                    this.NetworkObject.DefensePosition = newPlayerPosition;
                    this.NetworkObject.MidfieldPosition = newPlayerPosition;
                    this.NetworkObject.StrikerPosition = newPlayerPosition;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested = PositionBits.All;
                    }

                    break;
                case Bar.Keeper:
                    this.NetworkObject.KeeperPosition = newPlayerPosition;
                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.KeeperPosition;
                    }

                    break;
                case Bar.Defense:
                    this.NetworkObject.DefensePosition = newPlayerPosition;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.DefensePosition;
                    }

                    break;
                case Bar.Midfield:
                    this.NetworkObject.MidfieldPosition = newPlayerPosition;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.MidfieldPosition;
                    }

                    break;
                case Bar.Striker:
                    this.NetworkObject.StrikerPosition = newPlayerPosition;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.StrikerPosition;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("playerBar");
            }
            this.networkLayer.Send(this.NetworkObject);
            if (waitForResponse)
            {
                // TODO: Check return code in Packet
                this.networkLayer.Read();
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
            switch (bar)
            {
                case Bar.All:
                    this.NetworkObject.KeeperAngle = angle;
                    this.NetworkObject.DefenseAngel = angle;
                    this.NetworkObject.MidfieldAngel = angle;
                    this.NetworkObject.StrikerAngel = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested = PositionBits.All;
                    }

                    break;
                case Bar.Keeper:
                    this.NetworkObject.KeeperAngle = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.KeeperAngle;
                    }

                    break;
                case Bar.Defense:
                    this.NetworkObject.DefenseAngel = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.DefenseAngle;
                    }

                    break;
                case Bar.Midfield:
                    this.NetworkObject.MidfieldAngel = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.MidfieldAngle;
                    }

                    break;
                case Bar.Striker:
                    this.NetworkObject.StrikerAngel = angle;

                    if (waitForResponse)
                    {
                        this.NetworkObject.ReplyRequested |= PositionBits.StrikerAngle;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("bar");
            }
            this.networkLayer.Send(this.NetworkObject);
            if (waitForResponse)
            {
                // TODO: Check return code in Packet
                this.networkLayer.Read();
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
            while (true)
            {
                this.resetEvent.WaitOne();

                this.networkLayer.Send(this.NetworkObject);
                this.NetworkObject.ClearReplyRequested();

                Thread.Sleep(this.MessageIntervall);
            }
        }

        /// <summary>
        /// Handles the GameStarted event of the Game control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Game_GameStarted(object sender, EventArgs e)
        {
            this.resetEvent.Set();
        }

        /// <summary>
        /// Handles the GameStopped event of the Game control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Game_GameStopped(object sender, EventArgs e)
        {
            this.resetEvent.Reset();
        }
    }
}