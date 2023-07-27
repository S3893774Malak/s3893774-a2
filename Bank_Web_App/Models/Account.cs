using System.ComponentModel.DataAnnotations;

namespace Bank_Web_App.Models
{
    public class Account
    {
        [Key] // Add this attribute to specify the primary key
        public int AccountNumber { get; set; }

        [Required]
        [MaxLength(1)]
        public string AccountType { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }

}
