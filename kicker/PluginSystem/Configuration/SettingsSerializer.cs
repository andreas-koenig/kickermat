namespace PluginSystem.Configuration
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;

    /// <summary>
    /// Klasse, von der Abgeleitet werden kann, um eigene Einstellungen als XML-Datei zu speichern
    /// </summary>
    public abstract class SettingsSerializer
    {
        /// <summary>
        /// Loads the Settings from XML.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The configuration object.</returns>
        public static T LoadSettingsFromXml<T>(string fileName)
        {
            T settings = default(T);
            if (File.Exists(fileName))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                StreamReader sr = new StreamReader(fileName);
                try
                {
                    settings = (T)ser.Deserialize(sr);
                }
                catch (InvalidOperationException)
                {
                    settings = default(T);
                }
                finally
                {
                    sr.Close();
                }
            }

            // if deserialization failed
            if (Equals(settings, default(T)))
            {
                // check if a pulblic, parameter-less constructor exists
                ConstructorInfo[] constructors = typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                foreach (ConstructorInfo constructor in constructors)
                {
                    if (constructor.GetParameters().Length == 0)
                    {
                        settings = (T)Activator.CreateInstance(typeof(T));
                        break;
                    }
                }
            }

            return settings;
        }

        /// <summary>
        /// Saves the Settings to XML.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object.</typeparam>
        /// <param name="configurationObject">The configuration object.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void SaveSettingsToXml<T>(T configurationObject, string fileName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            DirectoryInfo storagePath = new FileInfo(fileName).Directory;
            if (storagePath != null)
            {
                if (storagePath.Exists == false)
                {
                    storagePath.Create();
                }
            }

            FileStream str = new FileStream(fileName, FileMode.Create);
            ser.Serialize(str, configurationObject);
            str.Close();
        }

        /// <summary>
        /// Transfers all property values from one class to another by comparing their name and types.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void TransferProperties<TSource, TDestination>(TSource source, TDestination destination)
        {
            PropertyInfo[] sourceProperties = typeof(TSource).GetProperties();
            PropertyInfo[] destinationProperties = typeof(TDestination).GetProperties();

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                bool itemCopied = false;
                foreach (PropertyInfo destinationProperty in destinationProperties)
                {
                    if ((destinationProperty.Name == sourceProperty.Name) &&
                        (destinationProperty.GetType() == sourceProperty.GetType()))
                    {
                        destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                        itemCopied = true;
                        break;
                    }
                }

                if (itemCopied == false)
                {
                    throw new PluginSystemException("Source property " + sourceProperty.Name + "not found in destination class");
                }
            }
        }
    }
}