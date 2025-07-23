using CarRentalAPIBusinessLayer;
using CarRentalDataAccessLayer;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpPost("AddCustomer", Name = "addNewCustomer")]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCustomer(CustomerDTO NewCustomerDTO)
        {
            if (NewCustomerDTO == null || string.IsNullOrEmpty(NewCustomerDTO.Name) ||
                string.IsNullOrEmpty(NewCustomerDTO.ContactInformation) ||
                string.IsNullOrEmpty(NewCustomerDTO.DriverLicenseNumber))
            {
                return BadRequest("Invalid Customer Data");
            }

            ClsCustomer customer = new ClsCustomer(NewCustomerDTO);

            if (customer.Save())
            {
                NewCustomerDTO.Id = customer.ID;
                return CreatedAtRoute("addNewCustomer", new { id = NewCustomerDTO.Id }, NewCustomerDTO);
            }
            else
            {
                return BadRequest("Failed to add customer");
            }
        }
    }
}
