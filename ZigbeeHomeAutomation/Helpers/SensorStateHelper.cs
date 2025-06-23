using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigbeeHomeAutomation.Models;

namespace ZigbeeHomeAutomation.Helpers
{
    public static class SensorStateHelper
    {
        public static string? GetSensorValue(string deviceName, string parameterName)
        {
            if (SensorStateStore.SensorValues.TryGetValue(deviceName, out var paramDict) &&
                paramDict.TryGetValue(parameterName, out var value))
            {
                return value?.ToString();
            }

            return null;
        }

        public static void PrintAllSensorStates()
        {
            Console.WriteLine("📡 Current Sensor States:");
            Console.WriteLine(new string('-', 40));

            foreach (var sensor in SensorStateStore.SensorValues)
            {
                string sensorName = sensor.Key;
                var parameters = sensor.Value;

                Console.WriteLine($"🔧 Sensor: {sensorName}");

                foreach (var kvp in parameters)
                {
                    Console.WriteLine($"   • {kvp.Key}: {kvp.Value}");
                }

                Console.WriteLine();
            }

            if (SensorStateStore.SensorValues.Count == 0)
            {
                Console.WriteLine("⚠️ No sensors currently tracked.");
            }

            Console.WriteLine(new string('-', 40));
        }
    }
}
