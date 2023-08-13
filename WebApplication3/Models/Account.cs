namespace WebApplication3.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string? AccountType { get; set; }
        public int? AccountNumber { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual List<Transaction>? Transaction { get; set; }
        public virtual List<BillPay>? BillPay { get; set; }
    }
}
