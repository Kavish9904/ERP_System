using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EmployeeRegisterAPI.Models
{
    public class EmployeeModel
    {
        [Key]  // Explicitly define EmployeeID as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures auto-increment behavior
        public int EmployeeID { get; set; }

        [Required]
        public string EmployeeName { get; set; } = string.Empty;

        [Required]
        public string Occupation { get; set; } = string.Empty;

        public string? ImageName { get; set; }

        [Required]
        public string TaskCategory { get; set; } = "General";

        [Required]
        public string TaskPriority { get; set; } = "Medium";

        [NotMapped] 
        public string? ImageSrc { get; set; }

        [NotMapped] 
        public IFormFile? ImageFile { get; set; }
    }
}