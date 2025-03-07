using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Sufficit.Gateway.Wavoip
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSufficitGatewayWavoip(this IServiceCollection services)
        {
            services.AddOptions<GatewayOptions>();

            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            // Definindo o local da configuração global
            // Importante ser dessa forma para o sistema acompanhar as mudanças no arquivo de configuração em tempo real 
            services.Configure<GatewayOptions>(configuration.GetSection(GatewayOptions.SECTIONNAME));

            // Capturando para uso local
            var options = configuration.GetSection(GatewayOptions.SECTIONNAME).Get<GatewayOptions>() ?? new GatewayOptions();

            services.AddSingleton<APIClientService>();
            return services;
        }
    }
}
