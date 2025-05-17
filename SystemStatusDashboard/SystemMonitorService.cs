using SystemStatusDashboard.Provider.Interfaces;
using System.Diagnostics;
using SystemStatusDashboard.Models;
using Microsoft.AspNetCore.SignalR;
using SystemStatusDashboard.Provider.Hubs;
using SystemStatusDashboard.Controllers;
using LibreHardwareMonitor;
using LibreHardwareMonitor.Hardware;

namespace SystemStatusDashboard
{
    public class SystemMonitorService : BackgroundService, ISystemMonitorService //meant to get cpu/memory info every few seconds
    {
        private readonly IHubContext<SystemStatusHub> _hubContext; //injected hub into service
        private readonly ILogger<SystemMonitorService> _logger;
        private readonly Computer _computer;
        public SystemMonitorService(IHubContext<SystemStatusHub> hubContext, ILogger<SystemMonitorService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
            };
            _computer.Open();
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken) //Lets application run in the background
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                await GetStatsAsync();
                await Task.Delay(5000, stoppingToken); // every 5 seconds
            }
        }

        public async Task<SystemStatusModel> GetStatsAsync() //Returns a Task. create a new var, model that uses our systemstatus model layout. 
        {
            var model = new SystemStatusModel();
            foreach (var hardware in _computer.Hardware) //iterate through every hardware in Computer.Hardware. update through each iteration
            {
                hardware.Update();

                foreach (var sensor in hardware.Sensors)
                {
                   
                    if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("CPU"))
                        model.CpuUsage = sensor.Value ?? 0; 
                    if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("CPU"))
                        model.CpuTemperature = sensor.Value ?? 0; //?? returns left if null. otherwise, return right
                    if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU"))
                        model.GpuUsage = sensor.Value ?? 0;
                    if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("GPU"))
                        model.GpuTemperature = sensor.Value ?? 0;
                    
                }

            }

            _logger.LogInformation($"Information Received:\nCPU Temp: {model.CpuTemperature}\nCPU %:{model.CpuUsage}\nGPU Temp: {model.GpuTemperature}\nGPU %: {model.GpuUsage}\n");
            await _hubContext.Clients.All.SendAsync("ReceiveStats", model); //this sends update to SignalR
            return model; //Task.FromResult is better to use if theres no await method (faster)
        }
    }
}
