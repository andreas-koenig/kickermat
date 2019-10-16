namespace Communication
{
    using global::Communication.NetworkConnections.Packets.Udp;
    using global::Communication.NetworkConnections.Packets.Udp.Enums;

    /// <summary>
    /// Interface to control the kicker with bars and players.
    /// </summary>
    public interface ICommunication
    {
        void Send(NetworkObject position);
    }
}
