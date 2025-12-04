# Employee Management System

A hierarchical employee management system implemented in C# using ADO.NET. This project demonstrates direct database interaction without ORM, handling recursive data structures (employee trees), and connection management.

## Technologies Used
* **.NET 8+** (Web API)
* **ADO.NET** (`Microsoft.Data.SqlClient`) for database interactions.
* **MS SQL Server** for data storage.
* **Recursion** for building the employee hierarchy tree.

## Getting Started

Follow these steps to set up and run the project locally.

### 1. Prerequisites
* .NET SDK installed.
* Microsoft SQL Server (LocalDB).
* SQL Server Management Studio (SSMS).

### 2. Database Setup
The project requires a SQL Server database. I have included a setup script.

1.  Open **SQL Server Management Studio (SSMS)**.
2.  Open the `db_script.sql` file located in the root of this repository.
3.  Run the script to create a new database named TestTaskDB, create the Employee table, and populate it with test data.

### 3. Configuration
Configure the connection string in `appsettings.json` to match your local environment.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(local);Initial Catalog=TestTaskDB;User ID=;Password=YOUR_REAL_PASSWORD;TrustServerCertificate=True;"
  }
}