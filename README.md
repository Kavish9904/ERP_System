# Employee Register & Task Management System

## Overview
This project is a **Task Management System** built as part of a technical assignment for **Avidian Technologies**. The system allows users to create, update, categorize, and prioritize tasks efficiently. It features **JWT authentication**, **real-time updates with SignalR**, and a **responsive UI** built with React.js.

## Features
### Backend (C# .NET 9, ASP.NET Core)
- **User Authentication**: Secure login and authentication using **JWT tokens**.
- **Task Management**: CRUD operations for tasks with **category** and **priority levels**.
- **Real-time Updates**: Implemented using **SignalR**.
- **Search & Filtering**: Users can filter tasks by **category, priority, and search terms**.
- **Error Handling & Logging**: Custom middleware for exception handling and **Serilog logging**.
- **Database Integration**: Uses **SQL Server** for persistent data storage.
- **RESTful API**: Well-structured endpoints following **best RESTful practices**.

### Frontend (React.js)
- **User-friendly UI**: Built with **React.js and Bootstrap**.
- **State Management**: Uses React state and hooks for handling data.
- **Real-time Updates**: Automatically updates the UI when tasks change.
- **Search & Filtering**: UI supports search and filtering based on category and priority.
- **Secure API Calls**: Uses **JWT tokens** for authentication.

## Technologies Used
- **Backend:** C# .NET 9, ASP.NET Core, Entity Framework Core, SignalR, Serilog, SQL Server
- **Frontend:** React.js, Bootstrap, Axios, SignalR Client
- **Authentication:** JWT (JSON Web Tokens)
- **Database:** Microsoft SQL Server
- **DevOps:** Docker (for containerization)

## Installation & Setup
### Prerequisites
Ensure you have the following installed on your system:
- .NET 9 SDK
- Node.js & npm
- SQL Server
- Git
- Docker (optional)

### Backend Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/Kavish9904/Avidian_TechAssignment.git
   cd Avidian_TechAssignment/TechAssignment
   ```
2. Set up the database connection in `appsettings.json`.
3. Run database migrations:
   ```bash
   dotnet ef database update
   ```
4. Start the API server:
   ```bash
   dotnet run
   ```

### Frontend Setup
1. Navigate to the frontend directory:
   ```bash
   cd employee-register-client
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the React app:
   ```bash
   npm start
   ```

## API Endpoints
| Method | Endpoint | Description |
|--------|---------|-------------|
| POST | `/api/auth/login` | User login (returns JWT) |
| GET | `/api/Employee` | Fetch all employees |
| POST | `/api/Employee` | Add a new employee |
| PUT | `/api/Employee/{id}` | Update employee details |
| DELETE | `/api/Employee/{id}` | Delete an employee |

## Time Spent on the Project
The project was developed in **1 week**, including:
- **2 days**: Backend development (API, database setup, authentication)
- **3 days**: Frontend development (UI, state management, API integration)
- **1 day**: SignalR implementation (real-time updates)
- **1 day**: Testing, debugging, and documentation

## Future Improvements
- Implement user roles and permissions.
- Add unit and integration tests.
- Deploy using **Docker** and **CI/CD pipeline**.

## License
This project is for educational and demonstration purposes.

---
For any questions or suggestions, feel free to reach out!


