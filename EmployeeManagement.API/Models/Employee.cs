namespace EmployeeManagement.API.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ManagerID { get; set; }
        public bool Enable { get; set; }

        // For a tree structure
        public List<Employee> Subordinates { get; set; } = new List<Employee>();
    }
}
