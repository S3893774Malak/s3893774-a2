[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPut("update-customer-profile/{customerId}")]
    public IActionResult UpdateCustomerProfile(int customerId, CustomerProfileUpdateRequest request)
    {
        // Update the customer's profile details
        var result = _adminService.UpdateCustomerProfile(customerId, request);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        return BadRequest(result.Message);
    }

    [HttpPost("lock-customer-login/{customerId}")]
    public IActionResult LockCustomerLogin(int customerId)
    {
        // Lock the customer's login
        var result = _adminService.LockCustomerLogin(customerId);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        return BadRequest(result.Message);
    }

    [HttpPost("unlock-customer-login/{customerId}")]
    public IActionResult UnlockCustomerLogin(int customerId)
    {
        // Unlock the customer's login
        var result = _adminService.UnlockCustomerLogin(customerId);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        return BadRequest(result.Message);
    }

    [HttpPost("block-scheduled-payment/{paymentId}")]
    public IActionResult BlockScheduledPayment(int paymentId)
    {
        // Block the scheduled payment
        var result = _adminService.BlockScheduledPayment(paymentId);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        return BadRequest(result.Message);
    }

    [HttpPost("unblock-scheduled-payment/{paymentId}")]
    public IActionResult UnblockScheduledPayment(int paymentId)
    {
        // Unblock the scheduled payment
        var result = _adminService.UnblockScheduledPayment(paymentId);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        return BadRequest(result.Message);
    }
}
