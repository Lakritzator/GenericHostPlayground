using System.IO;
using System.Reflection;

namespace Dapplo.Extensions.Plugins
{
    public class AssemblyScannerOptions
    {
        /// <summary>
        /// Indicates if AssemblyScanner status messages should be suppressed such as on startup.
        /// The default is false.
        /// </summary>
        public bool SuppressMessages { get; set; }

        /// <summary>
        /// Root of the location where the scanning takes place, default the location of the executing assembly
        /// </summary>
        public string Root { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Enable / disable plugin scanning
        /// </summary>
        public bool Scan { get; set; }

        /// <summary>
        /// The pattern to scan for plugins
        /// </summary>
        public string PluginPattern { get; set; }

        /// <summary>
        /// The relative paths for the plugins
        /// </summary>
        public string[] PluginPaths { get; set; }
    }
}
