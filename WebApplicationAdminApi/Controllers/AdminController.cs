using Microsoft.AspNetCore.Mvc;

namespace WebApplicationAdminApi.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            // Implement admin dashboard or other actions
            return View();
        }

        [HttpGet]
        public IActionResult ModifyProfile(int customerId)
        {
            // Retrieve customer profile by ID and display the modification form
            var customer = _adminService.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult ModifyProfile(CustomerProfileModel updatedCustomer)
        {
            // Update the customer's profile
            if (ModelState.IsValid)
            {
                _adminService.UpdateCustomerProfile(updatedCustomer);
                return RedirectToAction("Index");
            }
            return View(updatedCustomer);
        }

        [HttpPost]
        public IActionResult LockLogin(int customerId)
        {
            // Lock the customer's login
            _adminService.LockCustomerLogin(customerId);
            return RedirectToAction("Index");
        }

        // Implement similar actions for unlocking login and blocking/unblocking payments
    }
}
