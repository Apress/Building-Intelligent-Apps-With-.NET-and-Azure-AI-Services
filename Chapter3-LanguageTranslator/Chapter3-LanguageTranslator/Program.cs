using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure;
using Chapter3_LanguageTranslator.Business;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddAzureClients( builder =>
        {
            builder.AddTextTranslationClient(new AzureKeyCredential(Environment.GetEnvironmentVariable("TranslatorKey")), Environment.GetEnvironmentVariable("TranslatorRegion"));
        });
        services.AddSingleton<ITranslatorBusiness, TranslatorBusiness>();
    })
    .Build();

host.Run();
