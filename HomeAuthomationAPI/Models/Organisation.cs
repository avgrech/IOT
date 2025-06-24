using System.Collections.Generic;

namespace HomeAuthomationAPI.Models
{
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Property> Properties { get; set; } = new List<Property>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
