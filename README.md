 
 # Peddle DotNetCore  Clean Architecture Solution Template

This is a solution template for creating  ASP.NET Core Web API following the principles of Clean Architecture. You can create copy or this template and create new solution.

## Technologies

* .NET Core 3.1
* ASP .NET Core 3.1
* Entity Framework Core 3.1
* MediatR
* AutoMapper
* FluentValidation
* XUnit,Moq


### Database Configuration

This template require Database Connection configuration using EF Core , though it consist of Sample list of data  for Get and Post Methods.
Verify that the **DefaultConnection** connection string within **appsettings.json** points to a valid SQL Server instance. 

## Overview

### Domain

This will contain all entities, enums,Models,Dtos required for All layers.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### API

This Web API Layer. This layer depends on both the Application and Infrastructure layers, however, the dependencies of layers' services should be  inside *ServiceExtensions.cs*.


