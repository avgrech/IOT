using System;
using System.Collections.Generic;

namespace HomeAutomationBlazor.Models;

public class AutomationConfiguration
{
    public List<Rule> Rules { get; set; } = new();
    public DateTime ConfigurationDate { get; set; }
    public string ConfigurationName { get; set; } = string.Empty;
    public string RouterDeviceId { get; set; } = string.Empty;
}

public class Rule
{
    public List<Condition> conditon { get; set; } = new();
    public List<ActionDefinition> TrueConditiong { get; set; } = new();
    public List<ActionDefinition> FalseConditiong { get; set; } = new();
}

public class ActionDefinition
{
    public string deviceName { get; set; } = string.Empty;
    public string paramiterName { get; set; } = string.Empty;
    public string state { get; set; } = string.Empty;
    public AutomationDeviceType deviceType { get; set; } = AutomationDeviceType.Zigbee;
}

public class Condition
{
    public Sensor Sensor { get; set; } = new();
    public string state { get; set; } = string.Empty;
    public string timeStartParamiter { get; set; } = "00:00";
    public string timeEndParamiter { get; set; } = "23:59";
    public ComparasonOperator ComparasonOperator { get; set; }
}

public class Sensor
{
    public string deviceName { get; set; } = string.Empty;
    public string paramiterName { get; set; } = string.Empty;
    public string state { get; set; } = string.Empty;
    public AutomationDeviceType deviceType { get; set; } = AutomationDeviceType.Zigbee;
}

public enum AutomationDeviceType
{
    Zigbee,
    Tuya
}

public enum ComparasonOperator
{
    equalTo,
    GreaterThan,
    SmalerThan
}
