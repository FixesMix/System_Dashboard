using Microsoft.AspNetCore.SignalR;
namespace SystemStatusDashboard.Provider.Hubs
{
    public class SystemStatusHub : Hub //handles client connections
    {

       //Since a background service is pushing directly to connected clients, you dont need a method here
       //No client calls the hub first. Good for dashboards, live data feeds, push notifs
    }
}
