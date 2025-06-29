using System.ComponentModel.DataAnnotations;

namespace HomeAuthomationAPI.Models
{
    public class Parameter
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSensor { get; set; }
        public int DeviceId { get; set; }
        public Device? Device { get; set; }
    }
}
