using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sufficit.Gateway.Wavoip
{
    public abstract class ControllerSection
    {
        protected readonly IOptionsMonitor<GatewayOptions> ioptions;
        protected readonly IHttpClientFactory factory;
        protected readonly ILogger logger;
        protected readonly JsonSerializerOptions jsonOptions;

        public ControllerSection(IOptionsMonitor<GatewayOptions> ioptions, IHttpClientFactory factory, ILogger logger, JsonSerializerOptions jsonOptions)
        {
            this.ioptions = ioptions;
            this.factory = factory;
            this.logger = logger;
            this.jsonOptions = jsonOptions;
        }

        public ControllerSection(APIClientService service)
        {
            this.ioptions = service.ioptions;
            this.factory = service.factory;
            this.logger = service.logger;
            this.jsonOptions = service.jsonOptions;
        }

        #region TRICKS 

        protected HttpClient GetClient()
            => factory.Configure(options);

        protected GatewayOptions options
            => ioptions.CurrentValue;

        #endregion
    }
}
