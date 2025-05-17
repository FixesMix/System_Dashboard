namespace SystemStatusDashboard.Models
{
    public class SystemStatusModel // a class that represents structured data
    {
        public float CpuUsage { get; set; }
        public float CpuTemperature { get; set; }
        public float GpuUsage { get; set; }

        public float GpuTemperature { get; set; }
    }
}
