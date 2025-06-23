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

        Console.WriteLine("Connected to MQTT. Starting loop...");

        int delay = AppSettings.LoopIntervalSeconds;

        while (true)
        {
            try
            {
                Loop(); // your Arduino-style loop
                await Task.Delay(delay * 1000); // delay like delay(1/10) in Arduino (1/10 second)
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
}
