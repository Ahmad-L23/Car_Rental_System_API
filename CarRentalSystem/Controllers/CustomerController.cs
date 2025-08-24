using CarRentalAPIBusinessLayer;
using CarRentalDataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private bool IsValidCustomer(CustomerDTO customer)
        {
            if (customer == null) return false;
            if (string.IsNullOrEmpty(customer.Name)) return false;
            if (string.IsNullOrEmpty(customer.ContactInformation)) return false;
            if (string.IsNullOrEmpty(customer.DriverLicenseNumber)) return false;
            return true;
        }

        [HttpPost("AddCustomer", Name = "addNewCustomer")]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCustomer(CustomerDTO NewCustomerDTO)
        {
            if (!IsValidCustomer(NewCustomerDTO))
                return BadRequest("Invalid Customer Data");

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

        [HttpPut("UpdateCustomer")]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateCustomer(CustomerDTO UpdatedCustomerDTO)
        {
            if (UpdatedCustomerDTO?.Id == null || !IsValidCustomer(UpdatedCustomerDTO))
                return BadRequest("Invalid Customer Data");

            ClsCustomer customer = new ClsCustomer(UpdatedCustomerDTO, ClsCustomer.enMood.update);

            if (customer.Save())
            {
                return Ok(UpdatedCustomerDTO);
            }
            else
            {
                return BadRequest("Failed to update customer");
            }
        }


        [HttpDelete("DeleteCustomer/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCustomer(int id)
        {
            if (ClsCustomer.Delete(id))
            {
                return Ok($"Customer with ID {id} deleted successfully.");
            }
            return NotFound($"Customer with ID {id} not found.");
        }

        [HttpGet("GetCustomer/{id}")]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCustomerById(int id)
        {
            ClsCustomer? customer = ClsCustomer.GetById(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            return Ok(customer.CDTO);
        }

        [HttpGet("GetAllCustomers")]
        [ProducesResponseType(typeof(List<CustomerDTO>), StatusCodes.Status200OK)]
        public IActionResult GetAllCustomers()
        {
            List<ClsCustomer> customers = ClsCustomer.GetAllCustomers();
            List<CustomerDTO> dtos = new List<CustomerDTO>();
            foreach (var cust in customers)
                dtos.Add(cust.CDTO);

            return Ok(dtos);
        }

        [HttpGet("GetCustomersByName/{name}")]
        [ProducesResponseType(typeof(List<CustomerDTO>), StatusCodes.Status200OK)]
        public IActionResult GetCustomersByName(string name)
        {
            List<ClsCustomer> customers = ClsCustomer.GetByName(name);
            List<CustomerDTO> dtos = new List<CustomerDTO>();
            foreach (var cust in customers)
                dtos.Add(cust.CDTO);

            return Ok(dtos);
        }
    }
}
