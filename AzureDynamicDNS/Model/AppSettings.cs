using System;
using System.Collections.Generic;
using System.Text;

namespace AzureDynamicDNS.Model
{
    class AppSettings
    {
        public int updateinterval { get; set; }
        public List<string> PublicIPProviders { get; set; }
        public string PublicIPAddress { get; set; }
        public AzureSettings AzureSettings { get; set; }
    }
}
