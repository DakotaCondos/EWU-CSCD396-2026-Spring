# Assignment 2

This folder contains the containerized Razor Pages app used as the Assignment 2 deployment target.

For the Assignment 3 extension, the home page now includes a message form that publishes to Service Bus.

Required app settings for the message sender:

- `SERVICEBUS_CONNECTION_STRING`
- `SERVICEBUS_QUEUE_NAME`

In Azure, the app uses a Service Bus authorization rule connection string for sending messages.