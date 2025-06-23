using System.Collections.Concurrent;

namespace ZigbeeHomeAutomation.Models
{
    public static class SensorStateStore
    {
        public static ConcurrentDictionary<string, Dictionary<string, object>> SensorValues
            = new ConcurrentDictionary<string, Dictionary<string, object>>();
    }
}
