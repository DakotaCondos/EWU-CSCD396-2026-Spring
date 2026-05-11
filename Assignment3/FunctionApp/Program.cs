using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var storageAccountName = configuration["STORAGE_ACCOUNT_NAME"]
                ?? throw new InvalidOperationException("STORAGE_ACCOUNT_NAME is required.");

            var accountUri = new Uri($"https://{storageAccountName}.blob.core.windows.net");
            return new BlobServiceClient(accountUri, new DefaultAzureCredential());
        });
    })
    .Build();

host.Run();