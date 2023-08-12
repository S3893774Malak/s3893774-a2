namespace WebApplication3.Models
{
    public class Payee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Postcode { get; set; }
        public string? Mobile { get; set; }

        public virtual List<BillPay>? BillPays { get; set; }
    }
}
