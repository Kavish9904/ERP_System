using Microsoft.AspNetCore.SignalR;

namespace TechAssignment.Hubs // ✅ Ensure this matches your project namespace
{
    public class EmployeeHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task NotifyEmployeeAdded(string employeeName)
        {
            await Clients.All.SendAsync("EmployeeAdded", $"New employee added: {employeeName}");
        }

        public async Task NotifyEmployeeUpdated(string employeeName)
        {
            await Clients.All.SendAsync("EmployeeUpdated", $"Employee updated: {employeeName}");
        }

        public async Task NotifyEmployeeDeleted(string employeeName)
        {
            await Clients.All.SendAsync("EmployeeDeleted", $"Employee deleted: {employeeName}");
        }
    }
}
