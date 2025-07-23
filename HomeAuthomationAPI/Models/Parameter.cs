using System.ComponentModel.DataAnnotations;

namespace HomeAuthomationAPI.Models
{
    public class Parameter
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSensor { get; set; }
        public bool IsShown { get; set; }
        public int DeviceTypeId { get; set; }
        public DeviceType? DeviceType { get; set; }
    }
}
