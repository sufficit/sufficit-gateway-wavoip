using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sufficit.Gateway.Wavoip
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Nearly the HttpResponseMessage.EnsureSuccessStatusCode(), but reads the content from request before throws
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public static async ValueTask EnsureSuccess(this HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            if (!response.IsSuccessStatusCode)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var text = await response.Content.ReadAsStringAsync();
                string? message = null;
                
                if (!string.IsNullOrWhiteSpace(response.ReasonPhrase))
                    message = response.ReasonPhrase;

                HttpRequestException ex;
#if NET5_0_OR_GREATER
                 ex = new HttpRequestException(message, null, response.StatusCode);
#else
                ex = new HttpRequestException(message);
#endif
                ex.Data["statuscode"] = (int)response.StatusCode;
                ex.Data["method"] = response.RequestMessage?.Method.Method;
                // trying to deserialize !
                try
                {
                    ex.Data["content"] = JsonSerializer.Deserialize<dynamic>(text);
                }
                catch {
                    ex.Data["content"] = text;
                }

                // appending reponse headers
                ex.Data["headers"] = response.Headers.ToDictionary(s => s.Key, v => ToJsonValue(v.Value));
                throw ex;
            }
        }

        public static object? ToJsonValue(IEnumerable<string> values)
        {
            if (values.Count() <= 1)            
                return values.FirstOrDefault();
            
            return values;
        }

        public static HttpClient Configure(this IHttpClientFactory factory, GatewayOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.ClientId))
            {
                return factory.CreateClient(options.ClientId).Configure(options);
            } else {
                return factory.CreateClient().Configure(options);
            }
        }

        public static HttpClient Configure(this HttpClient source, GatewayOptions options)
        {
            if (options.TimeOut.HasValue)
                source.Timeout = TimeSpan.FromSeconds(options.TimeOut.Value);

            source.DefaultRequestHeaders.Add("User-Agent", options.Agent);
            return source;
        }
    }
}
