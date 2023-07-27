using System.ComponentModel.DataAnnotations;

namespace Bank_Web_App.Models
{
    public class BillPay
    {
        public int BillPayID { get; set; }
        public int AccountNumber { get; set; }
        public int PayeeID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime ScheduleTimeUtc { get; set; }
        [Required]
        [MaxLength(1)]
        public string Period { get; set; }

        public Account Account { get; set; }
        public Payee Payee { get; set; }
    }
}
