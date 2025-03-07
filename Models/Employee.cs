using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAdminPortal.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; } 
        public string? Phone { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public required decimal Salary { get; set; }
    }
}
