namespace Communication.PlayerControl
{
  using System;
  using GlobalDataTypes;
  using PluginSystem;

  /// <summary>
  /// Dummy class to simulate the controlling of players.
  /// </summary>
  public class DumymPlayerControl : IPlayerControl
  {
    /// <summary>
    /// Moves a player.
    /// </summary>
    /// <param name="playerBar">The player bar.</param>
    /// <param name="newPlayerPosition">The new player position.</param>
    public void MovePlayer(Bar playerBar, ushort newPlayerPosition)
    {
      // do nothing
    }

    /// <summary>
    /// Moves a player.
    /// </summary>
    /// <param name="playerBar">The player bar.</param>
    /// <param name="newPlayerPosition">The new player position.</param>
    /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
    public void MovePlayer(Bar playerBar, ushort newPlayerPosition, bool waitForResponse)
    {
      // do nothing
    }

    /// <summary>
    /// Sets the angle of a bar.
    /// </summary>
    /// <param name="bar">The bar which will be rotated.</param>
    /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
    public void SetAngle(Bar bar, short angle)
    {
      // do nothing
    }

    /// <summary>
    /// Sets the angle of a bar.
    /// </summary>
    /// <param name="bar">The bar which will be rotated.</param>
    /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
    /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
    public void SetAngle(Bar bar, short angle, bool waitForResponse)
    {
      // do nothing
    }
  }
}