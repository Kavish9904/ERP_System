using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeRegisterAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TechAssignment.Hubs;

namespace EmployeeRegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IHubContext<EmployeeHub> _hubContext;

        public EmployeeController(EmployeeDbContext context, IWebHostEnvironment hostEnvironment, ILogger<EmployeeController> logger, IHubContext<EmployeeHub> hubContext)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _hubContext = hubContext;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeModel>>> GetEmployees()
        {
            try
            {
                _logger.LogInformation("Fetching employees from database.");
                var employees = await _context.Employees.ToListAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching employees: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET: api/Employee/search?name=John
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeModel>>> SearchEmployees(string name)
        {
            var employees = await _context.Employees
                .Where(e => string.IsNullOrWhiteSpace(name) || e.EmployeeName.Contains(name))
                .ToListAsync();

            return Ok(employees);
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<EmployeeModel>> PostEmployeeModel([FromForm] EmployeeModel employeeModel)
        {
            try
            {
                _logger.LogInformation("Creating new employee");

                // if (employeeModel.ImageFile != null)
                //     employeeModel.ImageName = await SaveImage(employeeModel.ImageFile);
                // else
                //     employeeModel.ImageName = string.Empty;
                
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var imageFile = employeeModel.ImageFile;
                var filePath = Path.Combine(uploadPath, imageFile!.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                
                _context.Employees.Add(employeeModel);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "New employee added");
                return CreatedAtAction(nameof(GetEmployees), new { id = employeeModel.EmployeeID }, employeeModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating employee: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeModel(int id, [FromForm] EmployeeModel employeeModel)
        {
            if (id != employeeModel.EmployeeID)
                return BadRequest("Mismatched Employee ID");

            try
            {
                _logger.LogInformation($"Updating employee with ID {id}");
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                    return NotFound("Employee not found");

                existingEmployee.EmployeeName = employeeModel.EmployeeName;
                existingEmployee.Occupation = employeeModel.Occupation;
                existingEmployee.TaskCategory = employeeModel.TaskCategory;
                existingEmployee.TaskPriority = employeeModel.TaskPriority;

                if (!string.IsNullOrEmpty(existingEmployee.ImageName))
                    DeleteImage(existingEmployee.ImageName);

                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Employee updated");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating employee {id}: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeModel(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound("Employee not found");

            DeleteImage(employee.ImageName);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Employee deleted");
            return NoContent();
        }

        // Helper method to save image
        private async Task<string> SaveImage(IFormFile imageFile)
        {
            var imageName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", imageName);
            
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            
            return imageName;
        }

        // Helper method to delete image
        private void DeleteImage(string? imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", imageName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
        }
    }
}
