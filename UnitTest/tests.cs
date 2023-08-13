using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
using WebApplication.Controllers;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Tests
{
    public class AdminControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AdminControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task UpdateCustomerProfile_ValidData_ReturnsOkResult()
        {
            // Arrange
            var client = _factory.CreateClient();
            var customerId = 1; // Specify the customer ID
            var request = new CustomerProfileUpdateRequest
            {
                // Set properties for the request
                Name = "Updated Name",
                TFN = "Updated TFN",
                // Set other properties as needed
            };
            var mockAdminService = new Mock<IAdminService>();
            mockAdminService
                .Setup(service => service.UpdateCustomerProfile(customerId, request))
                .Returns(new ServiceResult { Success = true, Message = "Profile updated successfully." });
            var controller = new AdminController(mockAdminService.Object);

            // Act
            var result = await controller.UpdateCustomerProfile(customerId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Profile updated successfully.", okResult.Value);
        }

        // Write similar tests for the other endpoints
    }
}
