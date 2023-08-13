﻿namespace WebApplication3.Models
{
    public class Customer
    {
      public int Id { get; set; }

        public string? Name { get; set; }
        public string? Cid { get; set; }
        public string? TFN { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Postcode { get; set; }
        public string? Mobile { get; set; }
        public virtual List <Account>? Account { get; set; }
        public virtual Login? Login { get; set; }
    }
}
