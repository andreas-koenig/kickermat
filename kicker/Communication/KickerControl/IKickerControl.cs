namespace Communication.KickerControl
{
    using Communication.NetworkLayer.Packets.Udp;
    using Communication.NetworkLayer.Packets.Udp.Enums;
    using GameProperties;

    /// <summary>
    /// Interface to control the kicker with bars and players.
    /// </summary>
    public interface IKickerControl
    {
        void Send(NetworkObject position);
    }
}
