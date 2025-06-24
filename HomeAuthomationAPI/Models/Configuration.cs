namespace HomeAuthomationAPI.Models
{
    public class Configuration
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public string Content { get; set; } = string.Empty; // configuration file content
    }
}
