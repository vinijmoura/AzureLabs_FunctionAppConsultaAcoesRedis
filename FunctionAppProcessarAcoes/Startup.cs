using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunctionAppConsultaAcoesRedis.Data;

[assembly: FunctionsStartup(typeof(FunctionAppConsultaAcoesRedis.Startup))]
namespace FunctionAppConsultaAcoesRedis
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var builderConfig = new ConfigurationBuilder();
            builderConfig.AddAzureAppConfiguration(options =>
            {
                options.Connect(Environment.GetEnvironmentVariable("AppConfiguration"));
            });
            var configuration = builderConfig.Build();

            builder.Services.AddSingleton<AcoesRepository>(
                new AcoesRepository(configuration));
        }
    }
}