using System.Collections.Generic;

namespace HomeAuthomationAPI.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RouterDeviceId { get; set; } = string.Empty;
        public int OrganisationId { get; set; }
        public Organisation? Organisation { get; set; }
        public Configuration? Configuration { get; set; }

        public ICollection<RouterDevice> RouterDevices { get; set; } = new List<RouterDevice>();
    }
}
