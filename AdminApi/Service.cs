public interface IAdminService
{
    ServiceResult UpdateCustomerProfile(int customerId, CustomerProfileUpdateRequest request);
    ServiceResult LockCustomerLogin(int customerId);
    ServiceResult UnlockCustomerLogin(int customerId);
    ServiceResult BlockScheduledPayment(int paymentId);
    ServiceResult UnblockScheduledPayment(int paymentId);
}

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

   {
        try
        {
            // Retrieve the customer entity by customerId
            var customer = _unitOfWork.CustomerRepository.GetCustomerById(customerId);

            if (customer == null)
            {
                return ServiceResult.NotFound("Customer not found.");
            }

            // Update the customer's profile based on the request
            customer.Name = request.Name;
            customer.TFN = request.TFN;
            // Update other properties as needed

            // Save the changes to the database
            _unitOfWork.SaveChanges();

            return ServiceResult.Success("Customer profile updated successfully.");
        }
        catch (Exception ex)
        {
            // Log the error
            // Handle exceptions and return an appropriate ServiceResult indicating the error
            return ServiceResult.Error("An error occurred while updating the customer profile.");
        }
    }
}
