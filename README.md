# Porcupine User Management

---

## Getting Started

This project can be run locally in three ways: using your IDE, building the Docker image, or via Docker Compose.

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js + npm](https://nodejs.org/)
- [Angular CLI 19](https://angular.dev/tools/cli)
- (Optional) [Docker](https://www.docker.com/)
- (Optional) [Docker Compose](https://docs.docker.com/compose/)

---

## 🧩 Project Structure

```
/PorcupineUserManagement/ClientApp        # Angular frontend
/PorcupineUserManagement/                 # .NET Web API backend
/UnitTesting                              # .NET xUnit testing project
```
---

## 🛠️ Running Locally

### ✅ Option 1: Run from your IDE

1. Clone the repository:

   ```bash
   git clone https://github.com/StefanDL/porcupine-user-management.git
   cd porcupine-user-management
   ```

2. Update the database connection string in `appsettings.json`:

   ```json
   {
      "ConnectionStrings": {
        "ConnectionString": "Server=localhost;Database=PorcupineDb;User Id=sa;Password=Your_password123;"
      }
   }
   ```

3. From your IDE (e.g., Visual Studio / Rider), set the Web project as startup and run.

> ✅ The Angular UI is built and served automatically by the backend using SpaProxy or static build integration.

---

### 🐳 Option 2: Run via Docker
1. Clone the repository:

   ```bash
   git clone https://github.com/StefanDL/porcupine-user-management.git
   cd porcupine-user-management
   ```

2. Update the database connection string in `appsettings.json`:

   ```json
   {
      "ConnectionStrings": {
        "ConnectionString": "Server=localhost;Database=PorcupineDb;User Id=sa;Password=Your_password123;"
      }
   }
   ```
3. Build the image:

   ```bash
   docker build -t porcupine-user-mgmt .
   ```

4. Run the container:

   ```bash
   docker run -p 8080:80 porcupine-user-mgmt
   ```

---

### 🐙 Option 3: Run with Docker Compose

1. Run the app and SQL Server with:

   ```bash
   docker compose up --build
   ```

2. This will start:
    - The ASP.NET backend (serving the Angular UI)
    - SQL Server with the correct connection string preconfigured

3. Access the app at: [http://localhost:8080](http://localhost:8080)

---

## 🧪 Running Unit Tests

You can run backend unit tests using the .NET CLI:

```bash
dotnet test
```

> ✅ You can also integrate this into CI pipelines or extend Docker to run tests in containers.

