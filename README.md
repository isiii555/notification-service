# Notification Service - Task for TransferGo

## Project Overview

This is a **Notification Service** designed to send messages to customers using multiple messaging providers (e.g., Twilio, Amazon SNS, Vonage, etc.). The service abstracts the different providers and channels, allowing notifications to be sent through various means (SMS, Email, Push notifications). It is designed to be **extensible**, **resilient**, and **configurable**.

### Features
- **Multiple Messaging Providers**: Support for multiple providers (Twilio, Amazon SNS, Vonage), with the ability to switch or add new providers easily.
- **Various Channels**: Notifications can be sent through SMS, Email, Push notifications, and more.
- **Failover Mechanism**: If one provider fails, the service automatically switches to another provider with a higher priority. If all providers fail, the message will be delayed and retried when a provider becomes available again.
- **Configurable Providers**: Providers can be enabled/disabled and prioritized in a configuration file.
- **Domain-driven Design**: The solution is designed with a focus on clear domain logic, enabling easy extensions and maintenance.

### Goal
The goal of this project is to demonstrate my approach to building a flexible, maintainable, and resilient notification system. This system is designed with the following principles:
- **Domain-Driven Design** (DDD) for clear, modular architecture
- **Extensibility**: Easy to add new messaging providers or channels in the future
- **Resilience**: Graceful handling of failures with retries and failover mechanisms
- **Configurability**: Dynamic configuration of providers, channels, and priorities

---

## Key Components

### 1. **Messaging Providers**
The service supports multiple messaging providers (e.g., Twilio, Amazon SNS, Vonage). Each provider implements the `IMessageProvider` interface to send notifications.

#### Providers Supported:
- **Twilio**: For sending SMS and Email notifications.
- **Amazon SNS**: For sending SMS and Push notifications.
- **Vonage**: Placeholder for adding more providers in the future.

### 2. **Notification Channels**
The system supports multiple channels, each implementing the `IChannel` interface. The following channels are supported:
- **SMS**: Sending SMS messages to recipients.
- **Email**: Sending email notifications to recipients.
- **Push Notifications**: Placeholder for implementing Push notifications.

### 3. **Notification Service**
The `NotificationService` is responsible for sending notifications. It selects the appropriate provider based on the configured priority and channels available for each notification type. If a provider fails, the service retries or switches to the next available provider.

### 4. **Failover Mechanism**
The service uses a failover strategy where:
- If one provider fails, the system tries the next available provider.
- If all providers fail, the system will delay the notification and retry once a provider becomes available.

### 5. **Retry System**
A **background job** (`NotificationRetryJob`) has been implemented to periodically retry sending unsent notifications. The job fetches failed notifications from the database and attempts to resend them.

#### **Important Notes**:
- If the service is down, notifications may be lost as they are not persisted for retries.
- In a real-world scenario, **Outbox pattern** should be implemented to ensure that notifications are safely stored and can be retried when the system recovers.
  
The retry job runs every 5 minutes, attempting to send any unsent notifications and removing them from the database once successfully sent.

### 6. **Exception handling and packages**

- **Global Exception Handling**: All exceptions are globally handled to ensure proper error reporting and resilience.
- **Object Mapping with Mapster**: Mapster is used for efficient object-to-object mapping, reducing boilerplate code and improving maintainability.

### 7. **Configuration**
Providers, channels, priorities, and enable/disable statuses are configurable through the `appsettings.json` file.

### 8. **Logs**
In this project, currently, errors are logged using Console.WriteLine for simplicity and quick debugging.

In a real-world, production environment, it is recommended to use a proper logging system such as ILogger. This provides better log management, including logging to files, centralized systems, and more control over log levels (e.g., Info, Warning, Error). Transitioning to ILogger would ensure scalable, maintainable, and more robust logging practices.

#### Example Configuration (`appsettings.json`):
```json
{
  "MessagingProviders": {
    "Twilio": {
      "Enabled": true,
      "Priority": 1,
      "ApiKey": "twilioApiKey",
      "Channels": ["SMS", "Email"]
    },
    "AmazonSNS": {
      "Enabled": true,
      "Priority": 2,
      "ApiKey": "amazonSnsApiKey",
      "Channels": ["SMS", "Push"]
    }
  }
}
