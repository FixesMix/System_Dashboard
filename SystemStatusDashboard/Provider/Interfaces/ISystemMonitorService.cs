using SystemStatusDashboard.Models;

namespace SystemStatusDashboard.Provider.Interfaces
{
    public interface ISystemMonitorService
    {
        Task<SystemStatusModel> GetStatsAsync();
    }
}
