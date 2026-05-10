using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Assignment3.FunctionApp.Functions;

public sealed class ProcessServiceBusMessage
{
    private readonly BlobServiceClient blobServiceClient;
    private readonly IConfiguration configuration;
    private readonly ILogger<ProcessServiceBusMessage> logger;

    public ProcessServiceBusMessage(
        BlobServiceClient blobServiceClient,
        IConfiguration configuration,
        ILogger<ProcessServiceBusMessage> logger)
    {
        this.blobServiceClient = blobServiceClient;
        this.configuration = configuration;
        this.logger = logger;
    }

    [Function("ProcessServiceBusMessage")]
    public async Task Run(
        [ServiceBusTrigger("%SERVICEBUS_QUEUE_NAME%", Connection = "SERVICEBUS_CONNECTION")]
        string message,
        FunctionContext context)
    {
        var containerName = configuration["STORAGE_CONTAINER_NAME"] ?? "incoming-messages";
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobName = $"{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}.txt";
        var blobClient = containerClient.GetBlobClient(blobName);
        var payload = $"Timestamp: {DateTimeOffset.UtcNow:O}{Environment.NewLine}{Environment.NewLine}{message}";

        await using var payloadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
        await blobClient.UploadAsync(payloadStream, overwrite: true);

        logger.LogInformation("Stored Service Bus message in blob {BlobName} from invocation {InvocationId}.", blobName, context.InvocationId);
    }
}