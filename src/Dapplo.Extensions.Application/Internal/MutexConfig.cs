using System;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Extensions.Application.Internal
{
    /// <summary>
    /// This is the configuration for the mutex service
    /// </summary>
    internal class MutexConfig
    {
        /// <summary>
        /// The name of the mutex, usually a GUID
        /// </summary>
        public string MutexId { get; set; }
        
        /// <summary>
        /// This decides what prefix the mutex name gets, true will prepend Global\ and false Local\
        /// </summary>
        public bool IsGlobal { get; set; }

        /// <summary>
        /// The action which is called when the mutex cannot be locked
        /// </summary>
        public Action<IHostingEnvironment> WhenNotFirstInstance { get; set; }
    }
}
