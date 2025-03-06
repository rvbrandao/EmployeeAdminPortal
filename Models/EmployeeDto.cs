using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAdminPortal.Models
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }        
        public required decimal Salary { get; set; }
    }
}
