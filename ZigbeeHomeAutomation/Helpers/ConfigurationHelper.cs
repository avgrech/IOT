using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigbeeHomeAutomation.Models;

namespace ZigbeeHomeAutomation.Helpers
{

    public class ConfigurationHelper
    {
        public static Configuration GetLatestConfiguration(List<Configuration> configurations)
        {
            if (configurations == null || configurations.Count == 0)
                return null;

            return configurations
                .OrderByDescending(c => c.ConfigurationDate)
                .FirstOrDefault();
        }

        public static class RuleExecutionHelper
        {
            public static void CompileAndExecuteRules(Configuration configuration, RuleCompiler compiler)
            {
                if (configuration == null || configuration.Rules == null)
                {
                    Console.WriteLine("No configuration or rules to process.");
                    return;
                }

                Console.WriteLine($"Executing Configuration: {configuration.ConfigurationName} ({configuration.ConfigurationDate})");

                foreach (var rule in configuration.Rules)
                {
                    compiler.EvaluateRule(rule);
                }
            }
        }

    }
}
