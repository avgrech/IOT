using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZigbeeHomeAutomation.Helpers
{
    public static class TuyaClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string? _accessId;
        private static string? _accessSecret;
        private static string _endpoint = "https://openapi.tuyaus.com";
        private static string? _token;
        private static DateTime _tokenExpiry = DateTime.MinValue;

        public static async Task InitializeAsync()
        {
            Console.WriteLine("Initializing Tuya client.");
            _accessId = Environment.GetEnvironmentVariable("TUYA_ACCESS_ID");
            _accessSecret = Environment.GetEnvironmentVariable("TUYA_ACCESS_SECRET");
            var envEp = Environment.GetEnvironmentVariable("TUYA_ENDPOINT");
            if (!string.IsNullOrWhiteSpace(envEp))
            {
                _endpoint = envEp.TrimEnd('/');
            }

            if (string.IsNullOrEmpty(_accessId) || string.IsNullOrEmpty(_accessSecret))
            {
                Console.WriteLine("⚠️ Tuya credentials not configured. Set TUYA_ACCESS_ID and TUYA_ACCESS_SECRET.");
                return;
            }

            await EnsureTokenAsync();
        }

        public static Task<string?> GetSensorValue(string deviceId, string parameterName)
        {
            Console.WriteLine($"[Tuya] Getting {parameterName} for {deviceId}");
            return GetSensorValueInternal(deviceId, parameterName);
        }

        public static Task SendCommandAsync(string deviceId, string parameterName, string value)
        {
            Console.WriteLine($"[Tuya] Set {deviceId} {parameterName} to {value}");
            return SendCommandInternal(deviceId, parameterName, value);
        }

        private static async Task<string?> GetSensorValueInternal(string deviceId, string parameterName)
        {
            if (string.IsNullOrEmpty(_accessId) || string.IsNullOrEmpty(_accessSecret))
                return null;

            await EnsureTokenAsync();
            string path = $"/v1.0/devices/{deviceId}/status";
            string response = await SendAsync(HttpMethod.Get, path);

            dynamic? obj = JsonConvert.DeserializeObject(response);
            if (obj?.result == null) return null;
            foreach (var item in obj.result)
            {
                if (item.code == parameterName)
                {
                    return item.value?.ToString();
                }
            }
            return null;
        }

        private static async Task SendCommandInternal(string deviceId, string parameterName, string value)
        {
            if (string.IsNullOrEmpty(_accessId) || string.IsNullOrEmpty(_accessSecret))
                return;

            await EnsureTokenAsync();
            string path = $"/v1.0/devices/{deviceId}/commands";
            var payload = new
            {
                commands = new[]
                {
                    new { code = parameterName, value = value }
                }
            };

            await SendAsync(HttpMethod.Post, path, payload);
        }

        private static async Task EnsureTokenAsync()
        {
            if (!string.IsNullOrEmpty(_token) && DateTime.UtcNow < _tokenExpiry)
                return;

            var t = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signStr = _accessId + t;
            string sign = Sign(signStr);

            var req = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint}/v1.0/token?grant_type=1");
            req.Headers.Add("client_id", _accessId);
            req.Headers.Add("sign", sign);
            req.Headers.Add("t", t);
            req.Headers.Add("sign_method", "HMAC-SHA256");

            var resp = await _httpClient.SendAsync(req);
            string body = await resp.Content.ReadAsStringAsync();
            dynamic? obj = JsonConvert.DeserializeObject(body);
            _token = obj?.result?.access_token;
            long expire = obj?.result?.expire_time ?? 3600;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(expire - 60);
        }

        private static async Task<string> SendAsync(HttpMethod method, string path, object? body = null)
        {
            var url = _endpoint + path;
            var t = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string bodyStr = body == null ? string.Empty : JsonConvert.SerializeObject(body);
            string bodyHash = GetBodyHash(bodyStr);
            string signStr = _accessId + (_token ?? string.Empty) + t + method.Method.ToUpper() + "\n" + bodyHash + "\n" + "\n" + path;
            string sign = Sign(signStr);

            var req = new HttpRequestMessage(method, url);
            req.Headers.Add("client_id", _accessId);
            if (!string.IsNullOrEmpty(_token))
                req.Headers.Add("access_token", _token);
            req.Headers.Add("t", t);
            req.Headers.Add("sign", sign);
            req.Headers.Add("sign_method", "HMAC-SHA256");
            if (body != null)
            {
                req.Content = new StringContent(bodyStr, Encoding.UTF8, "application/json");
            }

            var resp = await _httpClient.SendAsync(req);
            return await resp.Content.ReadAsStringAsync();
        }

        private static string Sign(string content)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_accessSecret ?? string.Empty));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(content));
            return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
        }

        private static string GetBodyHash(string body)
        {
            if (string.IsNullOrEmpty(body)) return string.Empty;
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(body));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}

