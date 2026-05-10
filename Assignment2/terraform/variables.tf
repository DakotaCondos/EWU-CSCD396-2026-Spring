variable "resource_group_name" {
  type    = string
  default = "396_test"
}

variable "location" {
  type    = string
  default = "westus2"
}

variable "container_app_name" {
  type    = string
  default = "assignment2-container-app"
}

variable "container_image" {
  type = string
}

variable "acr_login_server" {
  type = string
}

variable "acr_username" {
  type = string
}

variable "acr_password" {
  type      = string
  sensitive = true
}

variable "servicebus_namespace_name" {
  type    = string
  default = "cscd396assn3sb"
}

variable "servicebus_namespace_resource_group_name" {
  type    = string
  default = "rg-assignment3"
}

variable "servicebus_queue_name" {
  type    = string
  default = "messages"
}