using System.Windows.Markup;

namespace wrcaysalesinventory.Data.Models
{
    public class CustomerModel
    {
        public string ID { get; set; }
        public string FullName { get => FirstName + " " + LastName; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Points { get; set; }
    }
}
