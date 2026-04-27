# User Management System

A modern full-stack application for user management with a .NET Core API backend and Angular frontend.

## 📋 Table of Contents
- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Installation](#installation)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Development](#development)

## 🎯 Overview

User Management System is a comprehensive solution for managing users with authentication, validation, and security best practices. It features:

- RESTful API built with ASP.NET Core
- Modern responsive UI with Angular
- JWT authentication
- Database migrations with Entity Framework Core
- Type-safe forms and validation
- Clean code architecture

## 🛠️ Tech Stack

### Backend
- **Framework**: ASP.NET Core (net10.0)
- **Database**: SQL Server / SQLite
- **ORM**: Entity Framework Core
- **Authentication**: JWT

### Frontend
- **Framework**: Angular 20
- **Language**: TypeScript
- **Styling**: CSS
- **State Management**: RxJS/Services

## 📋 Prerequisites

### Required
- **Node.js** 18+ (for Angular development)
- **.NET SDK** 10.0+ (for API)
- **npm** 9+ (comes with Node.js)
- **Git**

### Optional
- **Visual Studio** 2022+ or VS Code
- **SQL Server** (or use SQLite for development)
- **Postman** (for API testing)

## 📁 Project Structure

```
.
├── UserManagementAPI/           # Backend - ASP.NET Core API
│   ├── Controllers/             # API endpoints
│   ├── Models/                  # Data models
│   ├── Data/                    # DbContext
│   ├── Migrations/              # EF Core migrations
│   ├── appsettings.json         # Configuration
│   ├── Program.cs               # Application startup
│   └── UserManagementAPI.csproj # Project file
│
├── UserManagementUI/            # Frontend - Angular
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/      # UI components
│   │   │   ├── services/        # API services
│   │   │   └── app.routes.ts    # Routing
│   │   ├── index.html           # Entry point
│   │   └── main.ts              # Bootstrap
│   ├── angular.json             # Angular config
│   ├── package.json             # Dependencies
│   └── tsconfig.json            # TypeScript config
│
├── .gitignore                   # Git ignore rules
└── README.md                    # This file
```

## 🚀 Installation

### 1. Clone the Repository
```bash
git clone <repository-url>
cd "MiniprojGofive - Copy"
```

### 2. Setup Backend (UserManagementAPI)

```bash
cd UserManagementAPI

# Restore NuGet packages
dotnet restore

# Apply database migrations
dotnet ef database update

# Or for development with SQLite
dotnet ef database update -- --provider sqlite
```

**Configuration:**
- Copy `appsettings.Example.json` to `appsettings.Development.json` if needed
- Update connection string in `appsettings.json`

### 3. Setup Frontend (UserManagementUI)

```bash
cd UserManagementUI

# Install dependencies
npm install

# Install Angular CLI (if not installed globally)
npm install -g @angular/cli
```

## ▶️ Running the Application

### Start Backend API

```bash
cd UserManagementAPI

# Development mode with hot reload
dotnet watch run

# Or standard run
dotnet run
```

API runs on: `https://localhost:5001` or `http://localhost:5000`

### Start Frontend

```bash
cd UserManagementUI

# Development server
npm start

# Or with Angular CLI
ng serve
```

UI runs on: `http://localhost:4200`

### Access the Application

Open browser and navigate to: **http://localhost:4200**

## 📚 API Documentation

Browse API documentation at:
- **Swagger UI**: `https://localhost:5001/swagger`
- **Test requests**: See `UserManagementAPI/UserManagementAPI.http`

### Main Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users` | Get all users |
| GET | `/api/users/{id}` | Get user by ID |
| POST | `/api/users` | Create new user |
| PUT | `/api/users/{id}` | Update user |
| DELETE | `/api/users/{id}` | Delete user |

## 🔧 Development

### Making Changes

**Backend:**
```bash
cd UserManagementAPI
dotnet watch run
```

**Frontend:**
```bash
cd UserManagementUI
npm start
```

### Database Migrations

```bash
cd UserManagementAPI

# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Revert last migration
dotnet ef database update PreviousMigration
```

### Building for Production

**Backend:**
```bash
cd UserManagementAPI
dotnet publish -c Release
```

**Frontend:**
```bash
cd UserManagementUI
ng build --configuration production
```

## 🧪 Testing

**Backend:**
```bash
cd UserManagementAPI
dotnet test
```

**Frontend:**
```bash
cd UserManagementUI
npm test
```

## ⚠️ Important Notes

- **Never commit sensitive files:** 
  - `appsettings.Development.json` (database connection strings)
  - `.env` files
  - API keys or secrets

- These files are ignored by `.gitignore` - use local copies for development

- Use `appsettings.Example.json` as a template for local configuration

## 📝 Contributing

1. Create a feature branch: `git checkout -b feature/your-feature`
2. Commit changes: `git commit -am 'Add new feature'`
3. Push to branch: `git push origin feature/your-feature`
4. Submit a Pull Request

## 📄 License

This project is proprietary and confidential.

## 📧 Support

For issues or questions, please contact the development team.

---

**Framework Versions**: .NET 10, Angular 20, TypeScript 5
