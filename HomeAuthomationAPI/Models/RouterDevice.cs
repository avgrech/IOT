using System.Collections.Generic;

namespace HomeAuthomationAPI.Models
{
    public class RouterDevice
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; } = string.Empty;
        public string UniqueId { get; set; } = string.Empty;
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public ICollection<Device> Devices { get; set; } = new List<Device>();
        public ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
    }
}
