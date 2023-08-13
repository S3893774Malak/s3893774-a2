namespace WebApplicationAdminApi.Controllers
{
    public class CustomerProfileModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string TFN { get; set; }
        // Add other profile fields (Address, City, State, Postcode, Mobile, etc.)
    }
}
