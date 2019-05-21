namespace PluginSystem
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// User control which can control plugins.
    /// </summary>
    /// <typeparam name="TPlugin">The type of the plugin.</typeparam>
    public sealed partial class PluginSystemUserControl<TPlugin> : UserControl where TPlugin : class, IKickerPlugin
    {
        /// <summary>
        /// The parent class of the created plugin.
        /// </summary>
        private readonly object parentObject;

        /// <summary>
        /// Delegate to the init method which is called if the type of the plugin changes.
        /// </summary>
        private readonly Action<TPlugin> initMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSystemUserControl&lt;TPlugin&gt;"/> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <param name="selectedEntry">The selected entry.</param>
        /// <param name="initMethod">The init method.</param>
        public PluginSystemUserControl(object creator, Type selectedEntry, Action<TPlugin> initMethod)
        {
            this.InitializeComponent();
            this.parentObject = creator;
            this.initMethod = initMethod;
            Plugger.FillComboBoxWithTypes<TPlugin>(this.ComboBoxPluginTypes, selectedEntry);
        }

        /// <summary>
        /// Gets the plugin instance.
        /// </summary>
        /// <value>The plugin instance.</value>
        public TPlugin PluginInstance { get; private set; }       

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxPluginTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ComboBoxPluginTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBoxPluginTypes.SelectedItem != null)
            {
                if (this.PluginInstance != null)
                {
                    Plugger.DestroyInstance<TPlugin>(this.PluginInstance);
                    this.PluginInstance = null;
                }

                Type objectType = (Type)this.ComboBoxPluginTypes.SelectedItem;

                this.PluginInstance = Plugger.CreatePluginInstance<TPlugin>(this.parentObject, objectType);

                if (this.panelPlugin != null)
                {
                    this.panelPlugin.Controls.Clear();

                    if (this.PluginInstance.SettingsUserControl != null)
                    {
                        this.PluginInstance.SettingsUserControl.Dock = DockStyle.Fill;
                        this.panelPlugin.Controls.Add(this.PluginInstance.SettingsUserControl);
                    }
                }

                if (this.initMethod != null)
                {
                    // BUG ? Hier wird nur eine Methode übergeben, aber sourceImage nirgends gesetzt?
                    this.initMethod(this.PluginInstance);
                }
            }
        }
    }
}