# Assignment 2

This folder contains the containerized Razor Pages app used as the Assignment 2 deployment target.

For the Assignment 3 extension, the home page now includes a message form that publishes to Service Bus.

Required app settings for the message sender:

- `SERVICEBUS_NAMESPACE_FQDN`
- `SERVICEBUS_QUEUE_NAME`

In Azure, the app identity will also need the Service Bus sender role on the queue or namespace.