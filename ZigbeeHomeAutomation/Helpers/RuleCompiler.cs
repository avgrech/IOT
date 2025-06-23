using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigbeeHomeAutomation.Models;
using Action = ZigbeeHomeAutomation.Models.Action;

namespace ZigbeeHomeAutomation.Helpers
{
    public class RuleCompiler
    {
        private readonly Dictionary<string, Dictionary<string, bool>> _sensorValues;


        public void EvaluateRule(Rule rule)
        {
            bool allConditionsMet = true;

            foreach (var cond in rule.conditon)
            {
                if (!IsWithinTimeWindow(cond.timeStartParamiter, cond.timeEndParamiter))
                {
                    Console.WriteLine("⏱️ Time constraint not met.");
                    allConditionsMet = false;
                    break;
                }

                string currentValue = GetSensorState(cond.Sensor.deviceName, cond.Sensor.paramiterName);

                if (string.IsNullOrEmpty(currentValue))
                {
                    Console.WriteLine($"⚠️ No value found for {cond.Sensor.deviceName}/{cond.Sensor.paramiterName}");
                    allConditionsMet = false;
                    break;
                }

                var expected = cond.state;

                if (!Compare(currentValue, expected, cond.ComparasonOperator))
                {
                    allConditionsMet = false;
                    break;
                }
            }

            if (allConditionsMet)
            {
                foreach (var action in rule.TrueConditiong)
                {
                    ExecuteAction(action);
                }
            }


            //var actions = allConditionsMet ? rule.TrueConditiong : rule.FalseConditiong;

            //foreach (var action in actions)
            //{
            //    ExecuteAction(action);
            //}
        }


        private bool Compare(string actual, string expected, ComparasonOperator op)
        {
            actual = actual?.Trim();
            expected = expected?.Trim();

            return op switch
            {
                ComparasonOperator.equalTo =>
                    string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase),

                ComparasonOperator.GreaterThan =>
                    string.Compare(actual, expected, StringComparison.OrdinalIgnoreCase) > 0,

                ComparasonOperator.SmalerThan =>
                    string.Compare(actual, expected, StringComparison.OrdinalIgnoreCase) < 0,

                _ => false
            };
        }



        private int BoolToInt(bool val) => val ? 1 : 0;

        private string GetSensorState(string deviceName, string paramName)
        {

            string? actual = SensorStateHelper.GetSensorValue(deviceName, paramName);
            if (actual != null)
            {
                return actual;
            }

            Console.WriteLine($"Sensor state not found for {deviceName}:{paramName}");
            return "";
        }

        private async void ExecuteAction(Action action)
        {
            Console.WriteLine($"[ACTION] Setting {action.deviceName} with parameter {action.paramiterName} to {action.state}");
            if (Mqtt._mqttClient == null || !Mqtt._mqttClient.IsConnected)
            {
                Console.WriteLine("❌ MQTT client is not connected. Cannot send command.");
                return;
            }

            string topic = $"zigbee2mqtt/{action.deviceName}/set";

            var payload = new Dictionary<string, object>
        {
            { action.paramiterName, action.state }
        };

            string json = JsonConvert.SerializeObject(payload);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(json)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await Mqtt._mqttClient.PublishAsync(message);
            Console.WriteLine($"📤 Sent MQTT command → {topic}: {json}");

        }

        private bool IsWithinTimeWindow(TimeOnly start, TimeOnly end)
        {
            var now = TimeOnly.FromDateTime(DateTime.Now);

            if (start <= end)
                return now >= start && now <= end;
            else
                return now >= start || now <= end; // Handles overnight ranges
        }
    }

}
