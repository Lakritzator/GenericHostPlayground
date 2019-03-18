namespace GenericHostSample
{
    public class AssemblyScannerOptions
    {
        /// <summary>
        /// Indicates if AssemblyScanner status messages should be suppressed such as on startup.
        /// The default is false.
        /// </summary>
        public bool SuppressMessages { get; set; }

        /// <summary>
        /// The paths for the plugins
        /// </summary>
        public string [] PluginPaths { get; set; }
    }
}
