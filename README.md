# Library Management System API

This is a simple Library Management System API that provides the following endpoints on books:

- Lists All Books
- Creates a Book
- Finds a Book by Id
- Updates a Book
- Deletes a Book

It is built using ASP.NET Core and Entity Framework with SQLite as the in-built database.

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

## Option A: Using Visual Studio

2A: Open LibraryManagementSystemAPI.sln using Visual Studio IDE

3A: Once the project is open, press Ctrl + F5 to build the solution. 

## Option B: Using Command Line

2B. Install required NuGet packages

``` dotnet
dotnet restore
```

3B. Update the database

``` dotnet
dotnet ef database update
```

4B. Run the backend server

``` dotnet
dotnet run
```

The backend API will be available at https://localhost:7080

### API Documentation

API documentation is automatically generated using Swagger. Once the backend is running, you can view the API documentation by navigating to:

```
https://localhost:7080/swagger/index.html
```

This provides detailed information about the API endpoints and allows you to test the API directly from the browser. Also, you can find a PDF of the API Documentation in the root folder.

### Important Notes

- Database: The project uses an SQLite database. The database file will be automatically created when you run the migrations.

### Contributing

This is an educational project therefore, please feel free to suggest any improvements.

### License

This project is licensed under the MIT License.
