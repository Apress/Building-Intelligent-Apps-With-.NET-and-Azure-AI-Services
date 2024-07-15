using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Azure;
using Azure;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddAzureClients( builder =>
        {
            builder.AddBlobServiceClient(Environment.GetEnvironmentVariable("blobConn"));
            builder.AddTextAnalyticsClient(new Uri(Environment.GetEnvironmentVariable("textAnalyticsEndpoint")),
                new AzureKeyCredential(Environment.GetEnvironmentVariable("textAnalyticsKey"))
                );
        });
    })
    .Build();

host.Run();
