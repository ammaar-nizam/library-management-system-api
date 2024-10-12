# LibraryManagementSystemAPI

This is a simple Library Management System API that provides the following endpoints on books - GET All, GET by Id, POST, PUT and DELETE. It is built using ASP.NET Core and Entity Framework with SQLite as the database.

### Features

- Create, read, update, and delete books (CRUD operations)
- SQLite database for managing book data
- API documentation with Swagger

### Prerequisites

To run this project, you'll need the following:

- .NET 8.0 SDK or higher versions
- An IDE preferrably Visual Studio
- A web browser

### Setup Instructions

1. Clone the repository
     
``` git
git clone https://github.com/ammaar-nizam/library-management-system-api.git
cd library-management-system-api
```

2. Install required NuGet packages

``` dotnet
dotnet restore
```

3. Update the database

``` dotnet
dotnet ef database update
```

4. Run the backend server

``` dotnet
dotnet run
```

The backend API will be available at https://localhost:{port}

### API Documentation

API documentation is automatically generated using Swagger. Once the backend is running, you can view the API documentation by navigating to:

```
https://localhost:5001/swagger/index.html
```

This provides detailed information about the API endpoints and allows you to test the API directly from the browser.

### Important Notes

- Database: The project uses an SQLite database. The database file will be automatically created when you run the migrations.

### Contributing

This is an educational project therefore, please feel free to suggest any improvements.

### License

This project is licensed under the MIT License.
