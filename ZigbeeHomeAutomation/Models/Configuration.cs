using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigbeeHomeAutomation.Models
{
    public class Configuration
    {
        public List<Rule> Rules { get; set; }

        public DateTime ConfigurationDate {  get; set; }

        public string ConfigurationName { get; set; }

        public string RouterDeviceId { get; set; } = string.Empty;
    }
}
