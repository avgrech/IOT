using System.Collections.Generic;

namespace HomeAuthomationAPI.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RouterDeviceId { get; set; }
        public RouterDevice? RouterDevice { get; set; }
        public int DeviceTypeId { get; set; }
        public DeviceType? DeviceType { get; set; }
        public ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
        public ICollection<DeviceStatus> DeviceStatuses { get; set; } = new List<DeviceStatus>();
    }
}
