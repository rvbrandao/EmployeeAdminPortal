using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
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
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees.ToList();
            return Ok(allEmployees);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployees(Guid id)
        {
            var employeeEntity = dbContext.Employees.Find(id);
            if (employeeEntity is null)
            {
                return NotFound();
            }
            return Ok(employeeEntity);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeDto employeeDto)
        {
            var employeeEntity = new Employee()
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Salary = employeeDto.Salary
            };

            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, employeeEntity);
        }

        [HttpPut]
        public IActionResult UpdateEmployee(EmployeeDto employeeDto)
        {
            var employeeEntity = dbContext.Employees.Find(employeeDto.Id);
            if (employeeEntity is null)
            {
                return NotFound();
            }

            employeeEntity.Name = employeeDto.Name;
            employeeEntity.Email = employeeDto.Email;
            employeeEntity.Phone = employeeDto.Phone;
            employeeEntity.Salary = employeeDto.Salary;

            dbContext.SaveChanges();
            return Ok(employeeEntity);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employeeEntity = dbContext.Employees.Find(id);
            if (employeeEntity is null)
            {
                return NotFound();
            }

            dbContext.Employees.Remove(employeeEntity);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("search")]
        public IActionResult SearchEmployeesByName(string name)
        {
            var employees = dbContext.Employees
                .Where(e => EF.Functions.Like(e.Name, $"%{name}%"))
                .ToList();

            if (!employees.Any())
            {
                return NotFound(new { Message = $"No employees found with name containing '{name}'." });
            }

            return Ok(employees);
        }

        [HttpGet("complex-search")]
        public IActionResult SearchEmployeesByComplexQuery(string? name, decimal minSalary)
        {
            var sqlQuery = @"
                SELECT * 
                FROM Employees 
                WHERE (@name IS NULL OR Name LIKE {0})
                AND Salary >= {1}";

            

            var employees = dbContext.Employees
                 .FromSqlRaw(sqlQuery,
                    new SqlParameter("@name", name != null ? $"%{name}%" : (object)DBNull.Value),
                    new SqlParameter("@minSalary", minSalary))                
                .ToList();

            if (!employees.Any())
            {
                return NotFound(new { Message = $"No employees found with name containing '{name}' and salary >= {minSalary}." });
            }

            return Ok(employees);
        }

    }
}
