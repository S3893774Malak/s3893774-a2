using System.ComponentModel.DataAnnotations;

namespace Bank_Web_App.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(11)]
        public string TFN { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(40)]
        public string City { get; set; }
        [StringLength(3)]
        public string State { get; set; }
        [StringLength(4)]
        public string Postcode { get; set; }
        [StringLength(12)]
        public string Mobile { get; set; }

        // Add the Login property for the one-to-one relationship with Login entity
        public Login Login { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }

}
