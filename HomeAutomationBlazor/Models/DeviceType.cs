namespace HomeAutomationBlazor.Models;

public class DeviceType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Parameter>? Parameters { get; set; }
}
