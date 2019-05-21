namespace PluginSystem
{
    using System.Windows.Forms;

    /// <summary>
    /// Interface for a kicker software plugin.
    /// </summary>
    public interface IKickerPlugin
    {
        /// <summary>
        /// Gets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>
        UserControl SettingsUserControl { get; }       
    }
}