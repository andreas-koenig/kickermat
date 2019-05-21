namespace PluginSystem.Configuration
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Hilfs-Klasse zur Verwaltung der applikationsspezifischen Einstellungen.
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Gets the configuration files path.
        /// </summary>
        /// <value>The configuration files path.</value>
        public static string ConfigFilesPath
        {
            get
            {
                FileInfo applicationInfoa = new FileInfo(Application.ExecutablePath);
                string configurationFilesPath = applicationInfoa.DirectoryName;
                configurationFilesPath += Path.DirectorySeparatorChar;
                            
                // RoRe: aktuell werden alle Einstellungen im übergeordneten Verzeichnis gespeichert,
                // weil verschiedene Konfigurationen vorhanden sind und alle Konfigurationen die gleichen
                // Einstellungen benutzen sollen. 
                // Wenn das Programm fertig ist, kann der Pfad der exe-Datei verwendet werden
                // TODO: Erst im aktuellen, dann im übergeordneten Verzeichnis nach Settings
                // schauen, wenn nichts vorhanden im aktuellen Verzeichnis anlegen.
                configurationFilesPath += "..";
                configurationFilesPath += Path.DirectorySeparatorChar;
                configurationFilesPath += "Settings";
                configurationFilesPath += Path.DirectorySeparatorChar;

                return configurationFilesPath;
            }
        }

        /// <summary>
        /// Gets the relative path of a file according to an absolute path.
        /// </summary>
        /// <param name="absolutePath">The absolute path (of a directory).</param>
        /// <param name="relativeTo">The absolute path to a file or a directory.</param>
        /// <returns>The relative path of the file according to the absolute path.</returns>
        public static string GetRelativePath(string absolutePath, string relativeTo)
        {
            string[] absoluteDirectories = absolutePath.Split('\\');
            string[] relativeDirectories = relativeTo.Split('\\');

            // Get the shortest of the two paths
            int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

            // Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            // Find common root
            for (index = 0; index < length; index++)
            {
                if (absoluteDirectories[index] == relativeDirectories[index])
                {
                    lastCommonRoot = index;
                }
                else
                {
                    break;
                }
            }

            // If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
            {
                // TODO: Its better to return something instead of throwing
                throw new ArgumentException("Paths do not have a common base");
            }

            // Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            // Append the .. for parent folders
            for (index = lastCommonRoot + 1; index < absoluteDirectories.Length; index++)
            {
                if (absoluteDirectories[index].Length > 0)
                {
                    relativePath.Append("..\\");
                }
            }

            // Append the actual folders
            for (index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; index++)
            {
                relativePath.Append(relativeDirectories[index] + "\\");
            }

            relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);
            return relativePath.ToString();
        }
    }
}