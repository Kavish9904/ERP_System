using EmployeeRegisterAPI.Controllers;
using EmployeeRegisterAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TechAssignment.Hubs;

public class EmployeeControllerTests
{
    private readonly Mock<EmployeeDbContext> _mockContext;
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly Mock<ILogger<EmployeeController>> _mockLogger;
    private readonly Mock<IHubContext<EmployeeHub>> _mockHubContext; // ✅ Add this line
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
    {
        // Create mock EmployeeDbContext
        var options = new DbContextOptionsBuilder<EmployeeDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        _mockContext = new Mock<EmployeeDbContext>(options);

        // Mock IWebHostEnvironment
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockEnvironment.Setup(env => env.ContentRootPath).Returns("TestPath");

        // Mock ILogger<EmployeeController>
        _mockLogger = new Mock<ILogger<EmployeeController>>();

        // Mock IHubContext<EmployeeHub> for SignalR
        _mockHubContext = new Mock<IHubContext<EmployeeHub>>(); // ✅ Mock the HubContext

        // Set up an in-memory list of employees for testing
        var employeeData = new List<EmployeeModel>
        {
            new EmployeeModel { EmployeeID = 1, EmployeeName = "John", Occupation = "Developer" },
            new EmployeeModel { EmployeeID = 2, EmployeeName = "Jane", Occupation = "Manager" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<EmployeeModel>>();
        mockSet.As<IQueryable<EmployeeModel>>().Setup(m => m.Provider).Returns(employeeData.Provider);
        mockSet.As<IQueryable<EmployeeModel>>().Setup(m => m.Expression).Returns(employeeData.Expression);
        mockSet.As<IQueryable<EmployeeModel>>().Setup(m => m.ElementType).Returns(employeeData.ElementType);
        mockSet.As<IQueryable<EmployeeModel>>().Setup(m => m.GetEnumerator()).Returns(employeeData.GetEnumerator());

        _mockContext.Setup(c => c.Employees).Returns(mockSet.Object);

        // Initialize the controller with all dependencies
        _controller = new EmployeeController(
            _mockContext.Object,
            _mockEnvironment.Object,
            _mockLogger.Object,
            _mockHubContext.Object // ✅ Pass mocked IHubContext<EmployeeHub>
        );
    }

    // Test methods here
}
