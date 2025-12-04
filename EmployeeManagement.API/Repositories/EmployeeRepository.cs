using EmployeeManagement.API.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeManagement.API.Repositories
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // 1. GetEmployeeByID - returns the employee in a tree structure
        public Employee GetEmployeeTree(int id)
        {
            try
            {
                var employeeList = new List<Employee>();

                string query = @"
                WITH EmployeeTree AS (
                    SELECT ID, Name, ManagerID, Enable
                    FROM Employee
                    WHERE ID = @ID
                    UNION ALL
                    SELECT e.ID, e.Name, e.ManagerID, e.Enable
                    FROM Employee e
                    INNER JOIN EmployeeTree et ON e.ManagerID = et.ID
                )
                SELECT * FROM EmployeeTree";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employeeList.Add(new Employee
                                {
                                    ID = (int)reader["ID"],
                                    Name = reader["Name"]?.ToString() ?? string.Empty,
                                    ManagerID = reader["ManagerID"] as int?,
                                    Enable = (bool)reader["Enable"],
                                    Subordinates = new List<Employee>()
                                });
                            }
                        }
                    }
                }

                if (employeeList == null || employeeList.Count <= 0) 
                    return null;

                return BuildTree(employeeList, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to Get Employee Tree with ID: [{0}]. {1}", id, ex.Message));

                return null;
            }
        }

        // Helper method for building the tree
        private Employee BuildTree(List<Employee> employees, int id)
        {
            try
            {
                var employee = employees.FirstOrDefault(e => e.ID == id);
                if (employee == null || employee.ID <= 0) 
                    return null;

                var subordinates = employees.Where(e => e.ManagerID == employee.ID).ToList();
                if (subordinates != null && subordinates.Count > 0)
                {
                    foreach (var sub in subordinates)
                    {
                        if (sub != null && sub.ID > 0)
                            employee.Subordinates.Add(BuildTree(employees, sub.ID));
                    }
                }
                
                return employee;

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to Build Tree with Employee ID: [{0}]. {1}", id, ex.Message));

                return null;
            }
        }

        // 2. EnableEmployee - changes the Enable flag
        public void EnableEmployee(int id, bool enable)
        {
            try
            {
                string query = "UPDATE Employee SET Enable = @Enable WHERE ID = @ID";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@Enable", enable);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to Enable Employee with ID: [{0}]. {1}", id, ex.Message));

                throw;
            }
        }
    }
}
