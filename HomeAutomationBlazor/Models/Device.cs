namespace HomeAutomationBlazor.Models;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RouterDeviceId { get; set; }
    public int DeviceTypeId { get; set; }
    public DeviceType? DeviceType { get; set; }
}
