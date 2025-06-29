using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using static ZigbeeHomeAutomation.Helpers.ConfigurationHelper;
using ZigbeeHomeAutomation.Helpers;
using ZigbeeHomeAutomation.Models;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Raspberry Pi app...");

        await ZigbeeHomeAutomation.Helpers.Mqtt.ConnectToMqtt();
        await TuyaClient.InitializeAsync();

        Console.WriteLine("Connected to MQTT. Starting loop...");

        // Start the simple web server for health checks
        _ = Task.Run(() => WebServer.StartAsync());
        _ = Task.Run(() => ApiSyncLoop());
        _ = Task.Run(() => DirectMessageLoop());

        int delay = AppSettings.LoopIntervalSeconds;

        while (true)
        {
            try
            {
                Loop(); // your Arduino-style loop
                await Task.Delay(delay * 1000); // delay for the configured number of seconds
            }
            catch (Exception ex) { 
            Console.WriteLine(ex.ToString());
                await Task.Delay(5 * 1000); 
            }
        }
    }

    static void Loop()
    {
        Console.WriteLine($"Loop running at {DateTime.Now}");
        var allConfigs = ConfigurationFileLoader.LoadAllConfigurationsFromFolder();
        
        // Get the most recent configuration
        Configuration latestConfig = ConfigurationHelper.GetLatestConfiguration(allConfigs);


        //printing sensor states 
        SensorStateHelper.PrintAllSensorStates();

        var compiler = new RuleCompiler();

        // Compile and execute rules in the latest config
        RuleExecutionHelper.CompileAndExecuteRules(latestConfig, compiler);


    }

    static async Task ApiSyncLoop()
    {
        int delay = AppSettings.ApiSyncIntervalSeconds;
        while (true)
        {
            await HomeAutomationApiClient.SyncAsync();
            await Task.Delay(delay * 1000);
        }
    }

    static async Task DirectMessageLoop()
    {
        int delay = AppSettings.DirectMessageIntervalSeconds;
        var configs = ConfigurationFileLoader.LoadAllConfigurationsFromFolder();
        var latest = ConfigurationHelper.GetLatestConfiguration(configs);
        string routerId = latest?.RouterDeviceId ?? string.Empty;

        while (true)
        {
            await HomeAutomationApiClient.GetDirectMessagesAsync(routerId);
            await Task.Delay(delay * 1000);
        }
    }
}
