# Smart Learning - Backend API

## 📋 Project Overview

Smart Learning Backend is an ASP.NET Core Web API application built on Clean Architecture principles. This project provides a comprehensive API for an e-learning management system, with full support for authentication, course management, quizzes, virtual meetings, and more.

## 🏗️ Project Architecture (Clean Architecture)

The project is divided into four main layers:

```
SmartLearningProjectAPI.sln
│
├── SmartLearningProjectAPI (Presentation Layer)
│   ├── Controllers/          # API Controllers
│   ├── Middlewares/          # Custom Middlewares
│   ├── Program.cs            # Application Entry Point
│   └── appsettings.json      # Configuration
│
├── SmartLearning.Application (Application Layer)
│   ├── Services/             # Business Logic Services
│   ├── DTOs/                 # Data Transfer Objects
│   ├── Interfaces/           # Service Interfaces
│   ├── Mappings/             # AutoMapper Profiles
│   └── GenericInterfaces/    # Generic Repository Interfaces
│
├── SmartLearning.Core (Domain Layer)
│   ├── Model/                # Domain Entities
│   └── Enums/                # Domain Enumerations
│
└── SmartLearning.Infrastructure (Infrastructure Layer)
    ├── Data/                 # Database Context
    ├── Repositories/         # Data Access Repositories
    ├── ExternalServices/     # External Service Integrations
    └── Migrations/           # EF Core Migrations
```

### 📦 Layers and Responsibilities

#### 1. **SmartLearningProjectAPI** (Presentation Layer)
- **Responsibility**: Handle HTTP Requests/Responses
- **Main Components**:
  - 15 Controllers for various functionalities
  - Middleware for authentication and authorization
  - Swagger for interactive documentation

#### 2. **SmartLearning.Application** (Application Layer)
- **Responsibility**: Business logic and coordination between layers
- **Main Components**:
  - Services for business operations
  - DTOs for data transfer
  - AutoMapper for entity-DTO conversion

#### 3. **SmartLearning.Core** (Domain Layer)
- **Responsibility**: Define core entities and business rules
- **Main Components**:
  - 17 Domain Models (Entities)
  - Business Rules and constraints

#### 4. **SmartLearning.Infrastructure** (Infrastructure Layer)
- **Responsibility**: Data access and external services
- **Main Components**:
  - Entity Framework Core DbContext
  - Repository Pattern Implementation
  - External Service Integrations

## 🚀 Technologies Used

### Core Technologies
- **.NET 8.0** - Core Framework
- **ASP.NET Core Web API** - For building RESTful APIs
- **Entity Framework Core 8.0** - ORM for database operations
- **SQL Server** - Main database

### Security & Authentication
- **JWT Bearer Authentication** - For authentication and authorization
- **Microsoft.IdentityModel.JsonWebTokens** - For JWT handling
- **System.IdentityModel.Tokens.Jwt** - For creating and validating tokens

### Additional Packages
- **AutoMapper** - For entity-DTO conversion
- **Serilog** - For logging and monitoring
  - Serilog.AspNetCore
  - Serilog.Sinks.Console
  - Serilog.Sinks.File
- **Swashbuckle (Swagger)** - For interactive API documentation
- **CORS** - For cross-origin requests

## 📊 Data Models (Domain Models)

The project contains 17 main entities:

| Entity | Description |
|--------|-------------|
| **ApplicationUser** | System users |
| **Student** | Student data |
| **Instructor** | Instructor data |
| **Course** | Educational courses |
| **Unit** | Course units |
| **Lessons** | Lessons |
| **Quiz** | Quizzes |
| **Questions** | Quiz questions |
| **Choice** | Question choices |
| **StudentAnswer** | Student answers |
| **Enrollment** | Student course enrollments |
| **Grades** | Grades |
| **Attendance** | Attendance records |
| **Resource** | Educational resources |
| **Meeting** | Virtual meetings |
| **Payment** | Payments |
| **Rating** | Course ratings |

## 🎯 Controllers and Main Features

### 1. **AccountController**
- Register new users
- Login and logout
- Account and role management
- Password reset

### 2. **CourseController**
- Create, update, and delete courses
- List courses
- Search and filter
- Manage course content

### 3. **EnrollmentController**
- Enroll students in courses
- Unenroll students
- View enrolled courses
- Manage enrollment status

### 4. **LessonController**
- Create, update, and delete lessons
- Upload lesson content
- Order lessons within units
- Track student progress

### 5. **QuizController**
- Create quizzes
- Manage questions and choices
- Submit quizzes
- Grade and display results

### 6. **InstructorController**
- Manage instructor data
- View instructor courses
- Instructor statistics

### 7. **StudentController**
- Manage student data
- View student progress
- Student statistics

### 8. **UnitController**
- Create, update, and delete units
- Order units within courses
- Manage unit content

### 9. **ResourceController**
- Upload and manage files and resources
- Download resources
- Categorize resources (PDF, Video, Document, etc.)

### 10. **MeetingsController**
- Create virtual meetings
- Manage participants
- Schedule meetings

### 11. **AttendanceController**
- Record attendance
- View attendance records
- Attendance statistics

### 12. **ChatController**
- Manage messages and conversations
- Group and individual chat

### 13. **StreamController**
- Manage live streaming
- Integration with streaming services

### 14. **RolesController**
- Manage roles and permissions
- Assign roles to users

### 15. **WeatherForecastController**
- Demo controller (can be removed in production)

## ⚙️ Setup and Installation

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or Full Version)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Installation Steps

1. **Clone the repository**
```bash
git clone <repository-url>
cd Back
```

2. **Update Connection String**

Open `appsettings.json` in the `SmartLearningProjectAPI` project and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SmartLearningDB;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

3. **Apply Migrations**

```bash
cd SmartLearningProjectAPI
dotnet ef database update
```

Or from Package Manager Console in Visual Studio:
```powershell
Update-Database
```

4. **Run the Project**

```bash
dotnet run --project SmartLearningProjectAPI
```

Or from Visual Studio: Press F5

5. **Access the API**

- API Base URL: `https://localhost:7xxx` or `http://localhost:5xxx`
- Swagger UI: `https://localhost:7xxx/swagger`

## 🔐 Authentication and Authorization

The project uses **JWT Bearer Authentication**:

### Get Token

```http
POST /api/Account/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!"
}
```

### Use Token

```http
GET /api/Course
Authorization: Bearer {your-jwt-token}
```

### Available Roles

- **Admin** - Full permissions
- **Instructor** - Manage courses and content
- **Student** - Access courses and content


## 📊 Logging and Monitoring

The project uses **Serilog** for logging:

- **Console Logging**: For development
- **File Logging**: For production (in `Logs` folder)
- **Log Levels**: Information, Warning, Error, Debug

## 🔧 Configuration

```

## 🧪 Testing

### Using Swagger

1. Run the project
2. Open `https://localhost:7xxx/swagger`
3. Use Swagger UI to test APIs

### Using Postman

You can import a Postman Collection for quick testing.

## 🚀 Deployment

### Deploy to IIS

```bash
dotnet publish -c Release -o ./publish
```

### Deploy to Azure

```bash
az webapp up --name smart-learning-api --resource-group myResourceGroup
```

### Deploy using Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartLearningProjectAPI.dll"]
```

## 📚 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [JWT Authentication](https://jwt.io/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## 🤝 Contributing

To contribute to the project:
1. Fork the project
2. Create a new branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 📞 Contact

For questions and inquiries, please contact:
- Email: abdalaakhaleel564@gmail.com
- GitHub Issues: [Project Issues](https://github.com/your-repo/issues)

## 👥 Team Members

This project was developed by:
- **Abdalla Khalil**
- **Mennatullah Atef**
- **Hisham Radi**
- **Ramiz Magdy**

---

**Note**: This project is under active development. Some features may be in progress.
