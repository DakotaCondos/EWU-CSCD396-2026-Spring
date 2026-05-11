variable "resource_group_name" {
  type    = string
  default = "rg-assignment3"
}

variable "resource_group_location" {
  type    = string
  default = "westus2"
}

variable "location" {
  type    = string
  default = "canadacentral"
}

variable "function_app_prefix" {
  type    = string
  default = "assn3func"
}

variable "storage_account_prefix" {
  type    = string
  default = "assn3st"
}

variable "servicebus_namespace_name" {
  type    = string
  default = "cscd396assn3sb"
}

variable "servicebus_queue_name" {
  type    = string
  default = "messages"
}

variable "storage_container_name" {
  type    = string
  default = "incoming-messages"
}