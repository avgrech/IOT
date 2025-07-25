﻿using Microsoft.Extensions.Configuration;

namespace ZigbeeHomeAutomation.Helpers
{
    public static class AppSettings
    {
        public static IConfigurationRoot Configuration { get; }

        static AppSettings()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static string MqttIp => Configuration["Mqtt:BrokerIp"];
        public static int MqttPort => int.Parse(Configuration["Mqtt:BrokerPort"] ?? "1883");
        public static int LoopIntervalSeconds => int.Parse(Configuration["System:LoopIntervalSeconds"] ?? "5");
        public static int HttpPort => int.Parse(Configuration["HttpServer:Port"] ?? "8889");
        public static string ApiBaseUrl => Configuration["HomeAutomationApi:BaseUrl"] ?? string.Empty;
        public static int ApiSyncIntervalSeconds => int.Parse(Configuration["HomeAutomationApi:SyncIntervalSeconds"] ?? "30");
        public static int DirectMessageIntervalSeconds => int.Parse(Configuration["HomeAutomationApi:DirectMessageIntervalSeconds"] ?? "10");
        public static string ApiUsername => Configuration["HomeAutomationApi:Username"] ?? "admin";
        public static string ApiPassword => Configuration["HomeAutomationApi:Password"] ?? "admin";
    }
}
