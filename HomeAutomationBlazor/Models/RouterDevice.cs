namespace HomeAutomationBlazor.Models;

public class RouterDevice
{
    public int Id { get; set; }
    public string FriendlyName { get; set; } = string.Empty;
    public string UniqueId { get; set; } = string.Empty;
    public int PropertyId { get; set; }
}
