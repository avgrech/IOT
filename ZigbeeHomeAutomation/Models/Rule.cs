using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ZigbeeHomeAutomation.Models
{
    public class Rule
    {
        public List<Conditon> conditon { get; set; }
        public List<Action> TrueConditiong { get; set; }
        public List<Action> FalseConditiong { get; set; }
    }

    public class Action
    {
        public string deviceName { get; set; }
        public string paramiterName { get; set; }
        public string state { get; set; }
        public DeviceType deviceType { get; set; } = DeviceType.Zigbee;
    }

    public class Conditon
    {
        public sensor Sensor { get; set; }
        public string state { get; set; }
        public TimeOnly timeStartParamiter {  get; set; }
        public TimeOnly timeEndParamiter {  get; set; }
        public ComparasonOperator ComparasonOperator { get; set; }
    }

    public class sensor
    {
        public string deviceName { get; set; }
        public string paramiterName { get; set; }
        public string state { get; set; }
        public DeviceType deviceType { get; set; } = DeviceType.Zigbee;
    }

    public enum DeviceType
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
}
