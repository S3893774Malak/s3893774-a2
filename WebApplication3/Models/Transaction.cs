namespace WebApplication3.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string? TransactionType { get; set; }

        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }   
        public int DestinationAccountNumber { get; set; }
        public int Amount { get; set; }
        public string? Comment { get; set; }

        public string? TransactionTimeUTC { get; set; }

    }
}
