using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Assignment2.Pages;

public class IndexModel : PageModel
{
    private readonly ServiceBusClient serviceBusClient;
    private readonly IConfiguration configuration;
    private readonly ILogger<IndexModel> logger;

    public IndexModel(ServiceBusClient serviceBusClient, IConfiguration configuration, ILogger<IndexModel> logger)
    {
        this.serviceBusClient = serviceBusClient;
        this.configuration = configuration;
        this.logger = logger;
    }


    [BindProperty]
    public string Message { get; set; } = string.Empty;

    public string? StatusMessage { get; private set; }

    public bool IsError { get; private set; }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Message))
        {
            StatusMessage = "Enter a message before sending it to Service Bus.";
            IsError = true;
            return Page();
        }

        var queueName = configuration["SERVICEBUS_QUEUE_NAME"]
            ?? throw new InvalidOperationException("SERVICEBUS_QUEUE_NAME is required.");

        try
        {
            await using var sender = serviceBusClient.CreateSender(queueName);
            await sender.SendMessageAsync(new ServiceBusMessage(Message.Trim())
            {
                ContentType = "text/plain"
            });

            StatusMessage = "Message sent to Service Bus successfully.";
            IsError = false;
            Message = string.Empty;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to send message to Service Bus queue {QueueName}.", queueName);
            StatusMessage = "The message could not be sent right now. Check the app settings and Service Bus permissions.";
            IsError = true;
        }

        return Page();
    }
}
