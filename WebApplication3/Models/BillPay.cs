namespace WebApplication3.Models
{
    public class BillPay
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string? ScheduleTime { get; set; }
        public string? Period { get; set; }
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public int PayeeId { get; set; }
        public virtual Payee? Payee { get; set; }
    }
}
