# Assignment_ASP.NET

An ASP.NET Core MVC school project for managing projects, members, clients, authentication, and notifications.

## Overview

This repository is a school project built as a layered ASP.NET Core application with four projects:

- `WebApp` - MVC frontend, controllers, views, SignalR hubs, and startup configuration
- `Business` - business logic and services
- `Data` - Entity Framework Core context, entities, repositories, and migrations
- `Domain` - DTOs, domain models, and mapping helpers

The application uses:

- .NET 9
- ASP.NET Core MVC
- ASP.NET Core Identity
- Entity Framework Core
- SQLite
- SignalR
- Cookie-based authentication
- External authentication support

## Main Features

- User sign up and sign in
- Admin sign in
- Role-based access with `Administrator` and `User`
- Project management
- Member management
- Client management
- Real-time notifications with SignalR
- Simple chat page with SignalR
- SQLite database created automatically on startup
- Seeded default admin account

## Project Structure

```text
Assignment_ASP.NET/
|-- WebApp/
|-- Business/
|-- Data/
|-- Domain/
`-- Assignment_ASP.NET.sln
```
