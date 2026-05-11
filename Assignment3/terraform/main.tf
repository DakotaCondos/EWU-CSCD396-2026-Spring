resource "random_string" "suffix" {
  length  = 5
  upper   = false
  special = false
  numeric = true
}

locals {
  resource_suffix      = random_string.suffix.result
  storage_account_name = substr("${var.storage_account_prefix}${local.resource_suffix}", 0, 24)
  function_app_name    = substr("${var.function_app_prefix}-${local.resource_suffix}", 0, 60)
}

resource "azurerm_resource_group" "main" {
  name     = var.resource_group_name
  location = var.resource_group_location
}

resource "azurerm_storage_account" "functions" {
  name                     = local.storage_account_name
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "messages" {
  name                  = var.storage_container_name
  storage_account_name  = azurerm_storage_account.functions.name
  container_access_type = "private"
}

resource "azurerm_servicebus_namespace" "main" {
  name                = var.servicebus_namespace_name
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  sku                 = "Standard"
}

resource "azurerm_servicebus_queue" "messages" {
  name         = var.servicebus_queue_name
  namespace_id = azurerm_servicebus_namespace.main.id
}

resource "azurerm_servicebus_namespace_authorization_rule" "function_listener" {
  name         = "fn-listener"
  namespace_id = azurerm_servicebus_namespace.main.id
  listen       = true
  send         = false
  manage       = false
}

resource "azurerm_service_plan" "function" {
  name                = "asp-${local.function_app_name}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "main" {
  name                = local.function_app_name
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name

  service_plan_id            = azurerm_service_plan.function.id
  storage_account_name       = azurerm_storage_account.functions.name
  storage_account_access_key = azurerm_storage_account.functions.primary_access_key

  identity {
    type = "SystemAssigned"
  }

  site_config {
    application_stack {
      dotnet_version = "8.0"
    }
  }

  app_settings = {
    FUNCTIONS_WORKER_RUNTIME    = "dotnet-isolated"
    FUNCTIONS_EXTENSION_VERSION = "~4"
    WEBSITE_RUN_FROM_PACKAGE    = "1"
    SERVICEBUS_CONNECTION       = azurerm_servicebus_namespace_authorization_rule.function_listener.primary_connection_string
    SERVICEBUS_QUEUE_NAME       = azurerm_servicebus_queue.messages.name
    STORAGE_ACCOUNT_NAME        = azurerm_storage_account.functions.name
    STORAGE_CONTAINER_NAME      = azurerm_storage_container.messages.name
  }
}

data "azurerm_role_definition" "blob_data_contributor" {
  name = "Storage Blob Data Contributor"
}

resource "azurerm_role_assignment" "function_storage" {
  scope              = azurerm_storage_account.functions.id
  role_definition_id = data.azurerm_role_definition.blob_data_contributor.id
  principal_id       = azurerm_linux_function_app.main.identity[0].principal_id
}