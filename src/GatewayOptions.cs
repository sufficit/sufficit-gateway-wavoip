using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sufficit.Gateway.Wavoip
{
    public class GatewayOptions
    {
        public const string SECTIONNAME = nameof(Wavoip);

        public string UrlAPI { get; set; } = "https://api.wavoip.com";

        public string UrlDevices { get; set; } = "https://devices.wavoip.com";

        /// <summary>
        ///     Given by Wavoip dashboard to authenticate your requests
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        ///     Wavoip user for token requests
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        ///     Wavoip user password for token requests
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Default TimeOut (seconds) for endpoints requests 
        /// </summary>
        public uint? TimeOut { get; set; }

        public string Agent { get; set; } = "Sufficit C# API Client";

        /// <summary>
        /// Http Client Id
        /// </summary>
        public string? ClientId { get; set; }
    }
}
