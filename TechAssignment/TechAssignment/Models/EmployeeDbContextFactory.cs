// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;
// using System.IO;
//
// namespace EmployeeRegisterAPI.Models
// {
//     public class EmployeeDbContextFactory : IDesignTimeDbContextFactory<EmployeeDbContext>
//     {
//         public EmployeeDbContext CreateDbContext(string[] args)
//         {
//             var optionsBuilder = new DbContextOptionsBuilder<EmployeeDbContext>();
//
//             // Load configuration from appsettings.json
//             var configuration = new ConfigurationBuilder()
//                 .SetBasePath(Directory.GetCurrentDirectory())
//                 .AddJsonFile("appsettings.json")
//                 .Build();
//
//             // Use connection string from appsettings.json
//             optionsBuilder.UseSqlServer(configuration.GetConnectionString("DevConnection"));
//
//             return new EmployeeDbContext(optionsBuilder.Options);
//         }
//     }
// }
