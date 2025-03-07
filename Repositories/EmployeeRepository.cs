using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace EmployeeAdminPortal.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Employees.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> AddAsync(Employee employee)
        {
            employee.Id = Guid.NewGuid();
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var employee = await GetByIdAsync(id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Employee>> SearchByNameAsync(string name)
        {
            return await _dbContext.Employees
                .Where(e => e.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<List<Employee>> SearchByComplexQueryAsync(string? name, decimal minSalary)
        {
            var sqlQuery = @"
                    SELECT * 
                    FROM Employees 
                    WHERE (@name IS NULL OR Name LIKE {0})
                    AND Salary >= {1}";

            return await _dbContext.Employees
                .FromSqlRaw(sqlQuery,
                    new SqlParameter("@name", name != null ? $"%{name}%" : (object)DBNull.Value),
                    new SqlParameter("@minSalary", minSalary))
                .ToListAsync();
        }
    }
}
