using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigbeeHomeAutomation.Models;

namespace ZigbeeHomeAutomation.Helpers
{
    public class Mqtt
    {

        public static IMqttClient _mqttClient;
        public static MqttClientOptions _mqttOptions;

        public static async Task ConnectToMqtt()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _mqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(AppSettings.MqttIp, AppSettings.MqttPort) // Or use the Pi’s IP if running from another machine
                .Build();

            _mqttClient.ConnectedAsync += async e =>
            {
                Console.WriteLine("Connected to MQTT broker.");

                // Subscribe after connection
                await _mqttClient.SubscribeAsync("zigbee2mqtt/#");
            };

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                string topic = e.ApplicationMessage.Topic;
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                if (!topic.StartsWith("zigbee2mqtt/")) return;

                // Ignore bridge topics
                if (topic.StartsWith("zigbee2mqtt/bridge")) return;

                // Extract device name (e.g., "PersonSensor" from "zigbee2mqtt/PersonSensor/occupancy")
                string[] parts = topic.Split('/');
                if (parts.Length < 2) return;

                string deviceName = parts[1];

                try
                {
                    // Try parsing full object (for JSON messages)
                    var values = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
                    if (values != null)
                    {
                        SensorStateStore.SensorValues[deviceName] = values;
                    }
                    else
                    {
                        // Handle simple payloads (e.g., "ON", "true", 25)
                        SensorStateStore.SensorValues.AddOrUpdate(deviceName,
                            new Dictionary<string, object> { [parts.Last()] = payload },
                            (key, existing) =>
                            {
                                existing[parts.Last()] = payload;
                                return existing;
                            });
                    }

                    Console.WriteLine($"✅ Updated: {deviceName} → {payload}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error handling message for {topic}: {ex.Message}");
                }

                await Task.CompletedTask;
            };


            await _mqttClient.ConnectAsync(_mqttOptions);
        }
    }
}
