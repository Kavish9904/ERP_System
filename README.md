# Employee Register & Task Management System

## Overview

This is a full-stack Employee Register & Task Management System built using:

- **Backend**: C# .NET 9 (REST API, SignalR for real-time updates, JWT authentication, SQL Server/MongoDB for data persistence)
- **Frontend**: React.js (SPA with state management, real-time updates, search & filter functionality)
- **DevOps**: Docker for containerization, CI/CD-friendly setup

## Features

### **Employee Management**

- CRUD operations for employees
- Image upload & automatic saving to an `Images` folder

### **Task Management**

- Create, update, and track tasks
- Categorize tasks and assign priority levels
- Search and filter tasks

### **Real-time Updates**

- SignalR-based live updates for task changes
- WebSocket communication for seamless synchronization

### **Security**

- JWT authentication for secure API access
- Proper error handling and logging

---

## **Getting Started**

### **1. Prerequisites**

Ensure you have the following installed:

- **.NET 9 SDK**
- **Node.js (Latest LTS version)**
- **SQL Server or MongoDB**
- **Docker** (if using containers)

### **2. Clone the Repository**

```sh
git clone <repository-url>
cd TechAssignment  # Backend
```

### **3. Setup & Run Backend**

#### **Environment Variables**

Create a `.env` file in `TechAssignment` with the following:

```env
ASPNETCORE_ENVIRONMENT=Development
JWT_SECRET=your_jwt_secret
DATABASE_CONNECTION_STRING=your_database_connection
```

#### **Run Backend**

```sh
dotnet restore
dotnet run
```

This starts the backend at **`http://localhost:5150`** (or another port as per `launchSettings.json`).

---

### **4. Setup & Run Frontend**

```sh
cd ../employee-register-client  # Navigate to frontend
npm install
npm start
```

This starts the frontend at **`http://localhost:3000`**.

---

## **API Endpoints**

### **Employee API** (`/api/Employee`)

- `GET /api/Employee/` → Fetch all employees
- `POST /api/Employee/` → Add a new employee
- `PUT /api/Employee/{id}` → Update employee
- `DELETE /api/Employee/{id}` → Remove employee
- `POST /api/Employee/uploadImage` → Upload employee image

### **Task API** (`/api/Task`)

- `GET /api/Task/` → Fetch all tasks
- `POST /api/Task/` → Create a new task
- `PUT /api/Task/{id}` → Update task
- `DELETE /api/Task/{id}` → Remove task

### **Real-time Communication (SignalR)**

- `/employeehub` → Provides real-time task updates

---

## **Troubleshooting**

### **Backend Issues**

- **Check if the API is running**: Visit `http://localhost:5150/swagger` to verify.
- **CORS issues?** Ensure this is in `Program.cs`:
  ```csharp
  builder.Services.AddCors(options =>
  {
      options.AddPolicy("AllowAll", policy =>
      {
          policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
      });
  });
  app.UseCors("AllowAll");
  ```

### **Frontend Issues**

- **Connection refused?** Ensure backend is running and API URL is correct in React.
- **Run frontend with proper backend URL:**
  ```javascript
  const API_BASE_URL = "http://localhost:5150/api";
  axios.get(`${API_BASE_URL}/Employee/`);
  ```

### **Real-time (SignalR) Issues**

- Ensure `app.MapHub<EmployeeHub>("/employeehub");` is in `Program.cs`.
- Restart both backend and frontend (`Ctrl + C` in terminals, then `dotnet run` and `npm start`).

---

## **Deployment**

### **Docker Setup**

Ensure `docker-compose.yml` is configured for both frontend & backend:

```sh
docker-compose up --build
```

### **CI/CD & Production Deployment**

- Use **Azure App Service** or **AWS ECS** for deployment.
- Implement **GitHub Actions** for automated CI/CD.
