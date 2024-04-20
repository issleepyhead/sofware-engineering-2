using System.Security.Policy;

namespace wrcaysalesinventory.Data.Models
{
    public class UserModel
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DateAdded { get; set; }
        public string RoleName { get; set; }
    }
}
