namespace WebApplication3.Models
{
    public class Login
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public string? PasswordHash { get; set; }
    }
}
