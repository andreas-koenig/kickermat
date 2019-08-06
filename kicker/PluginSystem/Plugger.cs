namespace PluginSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using Configuration;

    /// <summary>
    /// Class with helper functions for the plugin system.
    /// </summary>
    public static class Plugger
    {
        /// <summary>
        /// Dictionary which contains a reference to the parent of each created plugin.
        /// The key is the created instance, the value is the parent of the instance.
        /// </summary>
        private static readonly Dictionary<object, object> childToParentLink = new Dictionary<object, object>();

        /// <summary>
        /// Creates a plugin user control.
        /// </summary>
        /// <typeparam name="TPlugin">The type of the plugin.</typeparam>
        /// <param name="creator">The creator of the user control.</param>
        /// <param name="defaultInstanceType">Default type of the instance.</param>
        /// <param name="parentControl">The parent control for the plugin user control.</param>
        /// <param name="initMethod">The init method which is called if the plugin type is changed.</param>
        /// <returns>The created plugin user control.</returns>
        public static PluginSystemUserControl<TPlugin> CreatePluginUserControl<TPlugin>(object creator, Type defaultInstanceType, Control parentControl, Action<TPlugin> initMethod)
            where TPlugin : class, IKickerPlugin
        {            
            PluginSystemUserControl<TPlugin> userControl = new PluginSystemUserControl<TPlugin>(creator, defaultInstanceType, initMethod);
            userControl.Dock = DockStyle.Fill;
            if (parentControl != null)
            {
                parentControl.Controls.Clear();
                parentControl.Controls.Add(userControl);
            }

            initMethod(userControl.PluginInstance);
            return userControl;
        }       

        /// <summary>
        /// Creates a plugin instance.
        /// </summary>
        /// <typeparam name="TPlugin">The type of the interface.</typeparam>
        /// <param name="creator">The instance parent which creates the instnace.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns>The plugin instance.</returns>
        public static TPlugin CreatePluginInstance<TPlugin>(object creator, Type instanceType)
        {
            TPlugin instance = (TPlugin)Activator.CreateInstance(instanceType);
            childToParentLink[instance] = creator;
            IXmlConfigurableKickerPlugin xmlConfigurablePlugin = instance as IXmlConfigurableKickerPlugin;
            if (xmlConfigurablePlugin != null)
            {
                string xmlFile = CreateXmlFileName(instance);
                
                // Load configuration always before initializing the user control
                xmlConfigurablePlugin.LoadConfiguration(xmlFile);
                //xmlConfigurablePlugin.InitUserControl();
            }
            
            return instance;
        }

        /// <summary>
        /// Fills the combo box with types.
        /// </summary>
        /// <typeparam name="TPlugin">The type of the plugin.</typeparam>
        /// <param name="usedComboBox">The used combo box.</param>
        /// <param name="selectedItem">The selected item.</param>
        public static void FillComboBoxWithTypes<TPlugin>(ComboBox usedComboBox, Type selectedItem)
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (typeof(TPlugin).IsAssignableFrom(t) && (t.IsInterface == false) && (t.IsAbstract == false))
                    {
                        usedComboBox.Items.Add(t);
                    }
                }
            }

            if (usedComboBox.Items.Count > 0)
            {
                if ((selectedItem != null) && usedComboBox.Items.Contains(selectedItem))
                {
                    usedComboBox.SelectedItem = selectedItem;
                }
                else
                {
                    usedComboBox.SelectedItem = usedComboBox.Items[0];
                }
            }
        }

        /// <summary>
        /// Destroys a plugin instance by saving its settings
        /// and calling IDisposable.Dispose() if implemented.
        /// </summary>
        /// <typeparam name="TPlugin">The type of the plugin.</typeparam>
        /// <param name="instance">The plugin instance.</param>
        public static void DestroyInstance<TPlugin>(TPlugin instance)
        {
            SavePluginSettings(instance);

            IDisposable disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Forces a kicker plugin to save its settings.
        /// </summary>
        /// <typeparam name="TPlugin">The type of the plugin.</typeparam>
        /// <param name="instance">The plugin instance.</param>
        public static void SavePluginSettings<TPlugin>(TPlugin instance)
        {
            IXmlConfigurableKickerPlugin kickerplugin = instance as IXmlConfigurableKickerPlugin;
            if (kickerplugin != null)
            {
                lock (kickerplugin)
                {
                    string xmlFile = CreateXmlFileName(kickerplugin);
                    kickerplugin.SaveConfiguration(xmlFile);
                }
            }
        }

        /// <summary>
        /// Creates the name of the XML file.
        /// </summary>
        /// <param name="instance">Instance which requires the xml file name.</param>
        /// <returns>The name of the xml file where the instance can store it's configuration.</returns>
        private static string CreateXmlFileName(object instance)
        {
            string xmlFileName = string.Empty;
            object tempInstance = instance;

            while (childToParentLink.ContainsKey(tempInstance) &&
                   childToParentLink.ContainsKey(childToParentLink[tempInstance]))
            {
                // Set parent name to the front of XmlFileName
                xmlFileName = childToParentLink[tempInstance].GetType().Name + Path.DirectorySeparatorChar + xmlFileName;
                tempInstance = childToParentLink[tempInstance];
            }

            xmlFileName = AppSettings.ConfigFilesPath + xmlFileName;
            xmlFileName += Path.DirectorySeparatorChar + instance.GetType().Name + ".xml";

            return xmlFileName;
        }
    }
}