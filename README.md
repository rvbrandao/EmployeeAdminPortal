# 🚀 Employee API - ASP.NET 8 + Entity Framework

This project is a CRUD API for managing employees, built with **ASP.NET 8** and **Entity Framework Core**. The application uses **Docker** for containerization, including the database, CI/CD with **Jenkins**, and database versioning with **Flyway**.

## 📌 Technologies Used
- **.NET 8** - Main framework for the API
- **Entity Framework Core** - ORM for database interaction
- **Docker** - For containerizing the application and dependencies
- **Database** - Microsoft SQL Server 2022 Express
- - **Flyway** - Database versioning and migrations
- **Jenkins** - CI/CD automation

## 📦 Project Structure
```
/EmployeeAPI
│── /controllers       # Application source code
│── /models            # Application source code
│── /data              # Application source code
│── /migrations        # Flyway-managed migration scripts
│── /docker            # Configuration and Dockerfiles
│── /jenkins           # Jenkins pipelines
│── docker-compose.yml # Container architecture
│── README.md          # Documentation
```

## 🚀 How to Run the Project Locally
### 1️⃣ Prerequisites
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### 2️⃣ Clone the Repository
```bash
git clone https://github.com/rvbrandao/employeeadminportal.git
cd employeeadminportal
```

### 3️⃣ Start the Containers
This will start the SQL Server and Jenkins.
```bash
docker-compose up -d
```

This will start the application.
```bash
docker build -t employees_api_v_1_0 .
docker run -d -p 9080:8080 --name employees_api --network dev-environment employees_api_v_1_0
```

### 4️⃣ Access the API
- **Base URL:** `http://localhost:9080/api/employees`
- **Swagger UI:** `http://localhost:9080/swagger`

## 🛠 Main Endpoints
| Method | Endpoint            | Description                 |
|--------|---------------------|-----------------------------|
| GET    | `/api/employees`    | Lists all employees        |
| GET    | `/api/employees/{id}` | Retrieves an employee by ID |
| GET    | `/api/employees/search` | Retrieves an employee by Name |
| GET    | `/api/employees/complex-search` | Retrieves an employee by Name or Salary |
| POST   | `/api/employees`    | Creates a new employee     |
| PUT    | `/api/employees/{id}` | Updates an employee       |
| DELETE | `/api/employees/{id}` | Deletes an employee       |

## 📜 Database Versioning

Database versioning is managed with **Flyway**. To manually run migrations:
```bash
docker run flyway/flyway -url="jdbc:sqlserver://host.docker.internal:1433;databaseName=EmployeesDb;user=sa;password=1StrongPwd!!;encrypt=false" migrate
```

## 🔄 CI/CD with Jenkins
The Jenkins pipeline performs:

✅ **Application build**  
✅ **Test execution**  
✅ **Docker image creation and publishing**  
✅ **API deployment**

Installing .NET
```bash
docker exec -u root -it jenkins01 /bin/bash

apt-get update && apt-get install -y apt-transport-https && apt-get install -y software-properties-common
curl -sSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/ubuntu/20.04/prod focal main" > /etc/apt/sources.list.d/dotnetdev.list'
apt-get update && apt-get install -y dotnet-sdk-8.0
```

You should install theses plugins:
- .NET SDK
- GitHub Pull Request Builder
- Post build task
- Docker pipeline
- Docker commons
- Docker