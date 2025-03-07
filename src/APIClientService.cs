using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sufficit.Gateway.Wavoip
{
    public class APIClientService : ControllerSection
    {
        public APIClientService(IOptionsMonitor<GatewayOptions> ioptions, IHttpClientFactory clientFactory, ILogger<APIClientService> logger)
            : base(ioptions, clientFactory, logger, Json.Options)
        {
            logger.LogTrace("Sufficit WaVoip Gateway API Client Service instantiated");
        }

        public Task<string> QrCode(CancellationToken cancellationToken)
            => !string.IsNullOrWhiteSpace(options.Token) ? QrCode(options.Token, cancellationToken) : throw new UnauthorizedAccessException("missing token");

        public async Task<string> QrCode(string token, CancellationToken cancellationToken)
        {
            using var client = GetClient();
            client.BaseAddress = new Uri(options.UrlDevices);

            var ep = "/:token/whatsapp/qr-image";
            ep = ep.Replace(":token", token);
            var uri = new Uri(ep, UriKind.Relative);

            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            using var response = await client.SendAsync(message, cancellationToken);
            await response.EnsureSuccess(cancellationToken);

            var text = await response.Content.ReadAsStringAsync();
            var qrcode = text.Split(',')[1];
            qrcode = qrcode.Split('"')[0];         
            
            return qrcode;
        }
    }
}
