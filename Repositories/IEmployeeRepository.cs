using EmployeeAdminPortal.Models;

namespace EmployeeAdminPortal.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> AddAsync(Employee employee);
        Task<bool> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(Guid id);
        Task<List<Employee>> SearchByNameAsync(string name);
        Task<List<Employee>> SearchByComplexQueryAsync(string? name, decimal minSalary);
    }
}
