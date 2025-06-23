using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZigbeeHomeAutomation.Models;
using Newtonsoft.Json;

namespace ZigbeeHomeAutomation.Helpers
{
    public static class WebServer
    {
        private static HttpListener? _listener;

        public static async Task StartAsync()
        {
            _listener = new HttpListener();
            string prefix = $"http://*:{AppSettings.HttpPort}/";
            _listener.Prefixes.Add(prefix);
            _listener.Start();
            Console.WriteLine($"🌐 Web server listening on {prefix}");

            while (true)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => HandleRequest(context));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"WebServer error: {ex.Message}");
                    await Task.Delay(1000);
                }
            }
        }

        private static void HandleRequest(HttpListenerContext context)
        {
            try
            {
                string html = GenerateStatusHtml();
                byte[] buffer = Encoding.UTF8.GetBytes(html);
                context.Response.ContentType = "text/html";
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }

        private static string GenerateStatusHtml()
        {
            var sb = new StringBuilder();
            sb.Append("<html><head><title>Zigbee Home Automation</title></head><body>");
            sb.Append("<h1>Zigbee Home Automation</h1>");
            sb.Append($"<p>Application running at {DateTime.Now}</p>");
            sb.Append("<h2>Sensor States</h2><ul>");

            foreach (var sensor in SensorStateStore.SensorValues)
            {
                string json = JsonConvert.SerializeObject(sensor.Value);
                sb.Append($"<li><b>{sensor.Key}</b>: {json}</li>");
            }
            if (SensorStateStore.SensorValues.Count == 0)
            {
                sb.Append("<li>No sensors tracked.</li>");
            }

            sb.Append("</ul></body></html>");
            return sb.ToString();
        }
    }
}
