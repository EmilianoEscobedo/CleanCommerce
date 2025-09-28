# CleanCommerce

CleanCommerce is a fully modular **microservices-based e-commerce** developed as final project for Web Applications Laboratory ISTEA assignature. <br/>

The application has been built with **ASP.NET Core 8**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.  It exposes **RESTful APIs** for the business logic and includes a **Blazor web app** as client UI application.

 
## Features

### Backend - Microservices Architecture

| Microservice | Responsibilities |
|-------------|-----------------|
| **Product** | CRUD for products (Name, Description, Price, Stock) |
| **Customer** | CRUD for customers (Name, Email, Address, Registration Date) | 
| **Order** | Create Orders from selected Products & Customer | 

Each microservice includes:

-  Own **relational database (SQL Server)**
-  **Clean Architecture + DDD** 
-  **Repository Pattern + Dependency Injection**
-  **CQRS using MediatR**
-  **Model Mapping with AutoMapper**
-  **Validation with FluentValidation**
-  **REST API with Swagger UI**
-  **HTTP Communication between Services**
-  **Global Exception Handling thru Middleware**
-  **Logging with Serilog**

### Frontend (UI Client)

The Web Client allows users to:

-  CRUD **Customers**
-  CRUD **Product Catalog**
-  CRUD **Orders**

## Technologies Used

| Category | Stack |
|----------|-------|
| Backend | ASP.NET Core 8, Entity Framework Core (SQL Server), MediatR, AutoMapper |
| Frontend | Blazor |
| Architecture | Clean Architecture, DDD, Repository Pattern, CQRS |
| Validation & Communication | FluentValidation, HTTP Clients |
| Observability | Serilog, Custom Middlewares |
| Containarization | Docker and docker-compose |

## Run the solution

### Run with Docker and Docker-Compose
You can run all microservices, the frontend UI, and the SQL Server database using **Docker** and **Docker Compose**.
Simply go to the root folder of the project and run:

```bash
docker-compose up -d
```

### Run manually
If you prefer, you can run the solution manually from your IDE or by CLI, using dotnet run commands.

#### Prerequisites

- .NET 8 SDK
- SQL Server
  - You will need to create a connection db on port 1433, with following details:
    - User Id=sa;
    - Password=Admin1234!

#### Run services manually

```bash
cd src/Customer.API
dotnet run

cd src/Order.API
dotnet run

cd src/Product.API
dotnet run

cd src/WebClient.API
dotnet run
```

### Getting Started

Now you should be able to see up and running the db, the 3 micro-services and the blazor webclient. Navigate to http://localhost:5164 in your browser to start trying the solution.
