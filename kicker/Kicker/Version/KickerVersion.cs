namespace Kicker.Version
{
    using System.Reflection;

    /// <summary>
    /// Static class for version information.
    /// </summary>
    public static partial class KickerVersion
    {
        /// <summary>
        /// Initializes static members of the KickerVersion class.
        /// </summary>
        static KickerVersion()
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            AssemblyName assname = ass.GetName();
            Major = assname.Version.Major;
            Minor = assname.Version.Minor;
        }

        /// <summary>
        /// Gets the current major version number.
        /// </summary>
        public static int Major { get; private set; }

        /// <summary>
        /// Gets the current minor version number.
        /// </summary>
        public static int Minor { get; private set; }
        
        /// <summary>
        /// Gets the complete version string.
        /// </summary>
        public static string Version
        {
            get
            {
                return Major + "." + Minor;
            }
        }
    }
}