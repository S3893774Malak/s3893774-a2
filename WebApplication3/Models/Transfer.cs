namespace WebApplication3.Models
{
    public class Transfer
    {
        public int PersonalAccountId { get; set; }
        public int DestinationAccountId { get; set; }
        public int Amount { get; set; }
        public string? Comment { get; set; }
    }
}
