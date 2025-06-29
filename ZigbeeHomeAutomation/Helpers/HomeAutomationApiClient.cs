using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

namespace ZigbeeHomeAutomation.Helpers
{
    public static class HomeAutomationApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private class SyncResponse
        {
            public string? ConfigurationContent { get; set; }
        }

        public static async Task SyncAsync()
        {
            var allConfigs = ConfigurationFileLoader.LoadAllConfigurationsFromFolder();
            var latest = ConfigurationHelper.GetLatestConfiguration(allConfigs);
            if (latest == null) return;

            var payload = new
            {
                routerDeviceId = latest.RouterDeviceId,
                deviceStatuses = SensorStateStore.SensorValues.ToDictionary(k => k.Key, k => k.Value)
            };

            var json = JsonConvert.SerializeObject(payload);
            try
            {
                var resp = await _httpClient.PostAsync($"{AppSettings.ApiBaseUrl}/sync",
                    new StringContent(json, Encoding.UTF8, "application/json"));
                if (!resp.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API sync failed: {resp.StatusCode}");
                    return;
                }

                var body = await resp.Content.ReadAsStringAsync();
                var respObj = JsonConvert.DeserializeObject<SyncResponse>(body);
                if (respObj?.ConfigurationContent != null)
                {
                    UpdateConfigurationIfNeeded(respObj.ConfigurationContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API sync error: {ex.Message}");
            }
        }

        private static void UpdateConfigurationIfNeeded(string newConfig)
        {
            string basePath = AppContext.BaseDirectory;
            string configPath = Path.Combine(basePath, "Configs", "configuration.json");

            try
            {
                string? existing = File.Exists(configPath) ? File.ReadAllText(configPath) : null;
                if (existing != newConfig)
                {
                    File.WriteAllText(configPath, newConfig);
                    Console.WriteLine("Updated configuration from server.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write configuration: {ex.Message}");
            }
        }
    }
}
