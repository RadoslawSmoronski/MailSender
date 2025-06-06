# MailSender API

## Overview

MailSender API is a RESTful service that allows client applications to register, send emails, and retrieve email sending logs. It uses JWT authentication to secure endpoints and logs email failures for traceability.

---

## API Documentation

You can find the full interactive API documentation here:  
[https://radoslawsmoronski.github.io/MailSender/](https://radoslawsmoronski.github.io/MailSender/)

---

## Features

- Register new client applications
- Send emails on behalf of authenticated applications
- Retrieve mail sending logs filtered by the authenticated application
- Error handling and logging
- JWT-based authentication and authorization

---

## Technologies Used

- .NET Web API
- AutoMapper
- MailKit (SMTP email sending)
- JWT Authentication
- Entity Framework Core with InMemory database provider
- Resend (email service integration)
- Swagger/OpenAPI for API documentation

---

## Authentication

The API uses JWT bearer tokens. Tokens must be included in the `Authorization` header as:

---

## Usage Example

#### Register Client App

    http
    POST /api/client-app/register
    Content-Type: application/json
    
    {
      "appId": "myAppId",
      "appName": "My Application",
      "pass": "securepassword"
    }

#### Send Email

    POST /api/mail/send
    Authorization: Bearer <token>
    Content-Type: application/json
    
    {
      "to": "recipient@example.com",
      "subject": "Test Email",
      "body": "<h1>Hello</h1><p>This is a test email.</p>"
    }

#### Get Mail Logs
    GET /api/mail/get-log
    Authorization: Bearer <token>

##Running the Project

- Clone the repository.
- Configure JWT signing keys and mail server settings in `appsettings.json`.
- Build and run the project.
- Access Swagger UI to explore and test the API.

## Configuration

### appsettings.json

Example configuration `appsettings.json`:

    json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "JWT": {
        "SigningKey": "your-secure-signing-key-here"
      },
      "BrevoSettings": {
        "Host": "smtp-relay.brevo.com",
        "Port": 587,
        "User": "your-brevo-smtp-user",
        "Password": "your-brevo-smtp-password",
        "SenderEmail": "your-email@example.com",
        "ApiKey": "your-brevo-api-key"
      },
      "ResendSettings": {
        "SenderEmail": "your-email@example.com",
        "ApiKey": "your-resend-api-key"
      },
      "AllowedHosts": "*"
    }
