namespace HomeAuthomationAPI.Models
{
    public class Configuration
    {
        public int Id { get; set; }

        public int? PropertyId { get; set; }
        public Property? Property { get; set; }

        public int? RouterDeviceId { get; set; }
        public RouterDevice? RouterDevice { get; set; }

        public int? DeviceId { get; set; }
        public Device? Device { get; set; }

        public string Content { get; set; } = string.Empty; // configuration file content
    }
}
