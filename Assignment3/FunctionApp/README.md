# Assignment 3 Function App

This Azure Function listens to a Service Bus queue and writes each received message to blob storage using a storage connection string.

## Configuration

Copy `local.settings.json.example` to `local.settings.json` for local development and fill in your Service Bus and storage values.

Required app settings in Azure:

- `FUNCTIONS_EXTENSION_VERSION=~4`
- `SERVICEBUS_CONNECTION`
- `SERVICEBUS_QUEUE_NAME`
- `STORAGE_CONNECTION_STRING`
- `STORAGE_CONTAINER_NAME`

## Behavior

- Service Bus trigger: `ProcessServiceBusMessage`
- Storage target: a container named by `STORAGE_CONTAINER_NAME`
- Each message is stored as a separate text blob