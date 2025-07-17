using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAuthomationAPI.Models
{
    public class DeviceStatus
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public Device? Device { get; set; }

        public DateTime Timestamp { get; set; }

        public string StatusJson { get; set; } = string.Empty;
    }

}
