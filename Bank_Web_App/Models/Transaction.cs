using System.ComponentModel.DataAnnotations;

namespace Bank_Web_App.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        [Required]
        [MaxLength(1)]
        public string TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int? DestinationAccountNumber { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [StringLength(30)]
        public string Comment { get; set; }
        [Required]
        public DateTime TransactionTimeUtc { get; set; }

        public Account Account { get; set; }
        public Account DestinationAccount { get; set; }
    }
}
