using System.Collections.Generic;

namespace HomeAuthomationAPI.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RouterDeviceId { get; set; }
        public RouterDevice? RouterDevice { get; set; }
        public ICollection<Parameter> Parameters { get; set; } = new List<Parameter>();
        public ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
    }
}
