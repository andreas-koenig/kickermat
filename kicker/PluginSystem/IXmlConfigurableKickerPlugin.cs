namespace PluginSystem
{
    /// <summary>
    /// Interface for modules which implement a xml configurable plugin.
    /// </summary>
    public interface IXmlConfigurableKickerPlugin
    {
        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        void LoadConfiguration(string xmlFileName);

        /// <summary>
        /// Saves the configuration to a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        void SaveConfiguration(string xmlFileName);

    }
}