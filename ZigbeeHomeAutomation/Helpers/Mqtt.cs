using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigbeeHomeAutomation.Models;
using Newtonsoft.Json;

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

                if (topic.StartsWith("zigbee2mqtt/bridge/event"))
                {
                    await HandleBridgeEvent(payload);
                    return;
                }

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

        private static async Task HandleBridgeEvent(string payload)
        {
            try
            {
                var evt = Newtonsoft.Json.JsonConvert.DeserializeObject<BridgeEvent>(payload);
                if (evt?.type == "device_joined" && evt.data?.friendly_name != null)
                {
                    await HomeAutomationApiClient.RegisterDeviceAsync(AppSettingsRouterId(), evt.data.friendly_name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bridge event error: {ex.Message}");
            }
        }

        private static string AppSettingsRouterId()
        {
            var configs = ConfigurationFileLoader.LoadAllConfigurationsFromFolder();
            var latest = ConfigurationHelper.GetLatestConfiguration(configs);
            return latest?.RouterDeviceId ?? string.Empty;
        }

        public static async Task SendCommand(string deviceName, string parameterName, string value)
        {
            if (_mqttClient == null || !_mqttClient.IsConnected) return;

            string topic = $"zigbee2mqtt/{deviceName}/set";
            var payload = new Dictionary<string, object>
            {
                [parameterName] = value
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(json)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await _mqttClient.PublishAsync(message);
        }

        public static async Task StartPairingAsync(int durationSeconds = 60)
        {
            if (_mqttClient == null || !_mqttClient.IsConnected) return;

            var builder = new MqttApplicationMessageBuilder()
                .WithTopic("zigbee2mqtt/bridge/request/permit_join");

            await _mqttClient.PublishAsync(builder.WithPayload("true").Build());
            await Task.Delay(durationSeconds * 1000);
            await _mqttClient.PublishAsync(builder.WithPayload("false").Build());
        }

        private class BridgeEvent
        {
            public string? type { get; set; }
            public BridgeEventData? data { get; set; }
        }

        private class BridgeEventData
        {
            public string? friendly_name { get; set; }
        }
    }
}
