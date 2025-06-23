using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ZigbeeHomeAutomation.Models;

namespace ZigbeeHomeAutomation.Helpers
{
    public class ConfigurationFileLoader
    {
        public static List<Configuration> LoadAllConfigurationsFromFolder()
        {
            string basePath = AppContext.BaseDirectory;
            string configsPath = Path.Combine(basePath, "Configs");

            if (!Directory.Exists(configsPath))
            {
                Console.WriteLine($"❌ Configs directory not found: {configsPath}");
                return new List<Configuration>();
            }

            var configFiles = Directory.GetFiles(configsPath, "*.json");
            var configurations = new List<Configuration>();

            foreach (var file in configFiles)
            {
                try
                {
                    string json = File.ReadAllText(file);

                    var settings = new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter> { new TimeOnlyJsonConverter() }
                    };

                    var config = JsonConvert.DeserializeObject<Configuration>(json, settings);

                    if (config != null)
                    {
                        configurations.Add(config);
                        Console.WriteLine($"✅ Loaded configuration: {config.ConfigurationName} ({config.ConfigurationDate}) from {Path.GetFileName(file)}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Failed to load '{file}': {ex.Message}");
                }
            }

            return configurations;
        }

        public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
        {
            private const string TimeFormat = "HH:mm";

            public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var timeString = (string)reader.Value;
                return TimeOnly.ParseExact(timeString, TimeFormat, CultureInfo.InvariantCulture);
            }

            public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString(TimeFormat));
            }
        }
    }
}
