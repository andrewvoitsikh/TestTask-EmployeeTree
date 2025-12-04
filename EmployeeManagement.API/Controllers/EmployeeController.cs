using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _repository;

        public EmployeeController(EmployeeRepository repository)
        {
            _repository = repository;
        }

        // GET: api/employee/1
        // Returns the employee tree for the given ID
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeByID(int id)
        {
            var employeeTree = _repository.GetEmployeeTree(id);

            if (employeeTree == null || employeeTree.ID == 0)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            return Ok(employeeTree);
        }

        // POST: api/employee/1/enable
        // Changes the Enable status. Takes the value true/false
        [HttpPost("{id}/enable")]
        public IActionResult EnableEmployee(int id, [FromBody] bool enable)
        {
            var employeeExists = _repository.GetEmployeeTree(id);
            if (employeeExists == null || employeeExists.ID == 0)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            _repository.EnableEmployee(id, enable);
            return Ok(new { Message = $"Employee {id} enable status set to {enable}" });
        }
    }
}
