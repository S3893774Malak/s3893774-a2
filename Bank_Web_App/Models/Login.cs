using System.ComponentModel.DataAnnotations;

namespace Bank_Web_App.Models
{
    public class Login
    {
        [Required]
        [StringLength(8)]
        public string LoginID { get; set; }
        public int CustomerID { get; set; }
        [Required]
        [StringLength(94)]
        public string PasswordHash { get; set; }

        public Customer Customer { get; set; }
    }
}
