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
            var provider = services.BuildServiceProvider(false);
            var configuration = provider.GetRequiredService<IConfiguration>();
            return services.AddSufficitGatewayWavoip(configuration);
        }

        public static IServiceCollection AddSufficitGatewayWavoip(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<GatewayOptions>();

            // Definindo o local da configuração global
            // Importante ser dessa forma para o sistema acompanhar as mudanças no arquivo de configuração em tempo real 
            services.Configure<GatewayOptions>(configuration.GetSection(GatewayOptions.SECTIONNAME));

            services.AddSingleton<APIClientService>();
            return services;
        }
    }
}
