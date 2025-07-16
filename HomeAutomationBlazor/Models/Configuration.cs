namespace HomeAutomationBlazor.Models;

public class Configuration
{
    public int Id { get; set; }
    public int? PropertyId { get; set; }
    public int? RouterDeviceId { get; set; }
    public int? DeviceId { get; set; }
    public string Content { get; set; } = string.Empty;
}
