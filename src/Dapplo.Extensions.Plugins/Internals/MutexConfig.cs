using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapplo.Extensions.Plugins.Internals
{
    internal class MutexConfig
    {
        public string MutexId { get; set; }
        public bool IsGlobal { get; set; } = true;

        public Action<IHostingEnvironment> WhenNotFirstInstance { get; set; }
    }
}
