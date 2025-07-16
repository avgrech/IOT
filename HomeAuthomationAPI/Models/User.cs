namespace HomeAuthomationAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsGlobalAdmin { get; set; }
        public int OrganisationId { get; set; }
        public Organisation? Organisation { get; set; }
    }
}
