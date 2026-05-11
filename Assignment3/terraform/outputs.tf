output "resource_group_name" {
  value = azurerm_resource_group.main.name
}

output "function_app_name" {
  value = azurerm_function_app_flex_consumption.main.name
}

output "storage_account_name" {
  value = azurerm_storage_account.functions.name
}

output "servicebus_namespace_name" {
  value = azurerm_servicebus_namespace.main.name
}

output "servicebus_namespace_fqdn" {
  value = format("%s.servicebus.windows.net", azurerm_servicebus_namespace.main.name)
}

output "servicebus_queue_name" {
  value = azurerm_servicebus_queue.messages.name
}