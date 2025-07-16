namespace HomeAutomationBlazor.Models;

public class DeviceStatus
{
    public int Id { get; set; }
    public string RouterDeviceId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string StatusJson { get; set; } = string.Empty;
}
