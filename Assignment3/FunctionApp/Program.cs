using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var storageConnectionString = configuration["STORAGE_CONNECTION_STRING"]
                ?? throw new InvalidOperationException("STORAGE_CONNECTION_STRING is required.");

            return new BlobServiceClient(storageConnectionString);
        });
    })
    .Build();

host.Run();