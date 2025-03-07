using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Controllers
{
    //localhost:xxx/api/employees
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController([FromServices] IEmployeeRepository _dbcontext)
        {
            employeeRepository = _dbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var allEmployees = await employeeRepository.GetAllAsync();
            return Ok(allEmployees);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var employeeEntity = await employeeRepository.GetByIdAsync(id);
            if (employeeEntity is null)
            {
                return NotFound();
            }
            return Ok(employeeEntity);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            var result = await employeeRepository.AddAsync(employee);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding employee.");
            }
            return StatusCode(StatusCodes.Status201Created, employee);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            var exists = await employeeRepository.ExistsAsync(employee.Id);
            if (!exists)
            {
                return NotFound();
            }

            var result = await employeeRepository.UpdateAsync(employee);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating employee.");
            }
            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var exists = await employeeRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var result = await employeeRepository.DeleteAsync(id);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting employee.");
            }
            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchEmployeesByName(string name)
        {
            var employees = await employeeRepository.SearchByNameAsync(name);
            if (employees.Count() == 0)
            {
                return NotFound(new { Message = $"No employees found with name containing '{name}'." });
            }

            return Ok(employees);
        }

        [HttpGet("complex-search")]
        public async Task<IActionResult> SearchEmployeesByComplexQuery(string? name, decimal minSalary)
        {
            var employees = await employeeRepository.SearchByComplexQueryAsync(name, minSalary);
            if (employees.Count() == 0)
            {
                return NotFound(new { Message = $"No employees found with name containing '{name}' and salary >= {minSalary}." });
            }

            return Ok(employees);
        }
    }
}
