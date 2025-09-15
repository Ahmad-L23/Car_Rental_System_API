using CarRentalAPIBusinessLayer;
using CarRentalDataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarFuelTypeController : ControllerBase
    {
        private bool IsValidFuelType(FuelTypeDTO fuelType)
        {
            if (fuelType == null) return false;
            if (string.IsNullOrEmpty(fuelType.FuelType)) return false;
            return true;
        }

        [HttpPost("AddFuelType", Name = "addNewFuelType")]
        [ProducesResponseType(typeof(FuelTypeDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddFuelType(FuelTypeDTO newFuelTypeDTO)
        {
            if (!IsValidFuelType(newFuelTypeDTO))
                return BadRequest("Invalid Fuel Type Data");

            ClsFuelType fuelType = new ClsFuelType(newFuelTypeDTO);

            if (fuelType.Save())
            {
                newFuelTypeDTO.Id = fuelType.ID;
                return CreatedAtRoute("addNewFuelType", new { id = newFuelTypeDTO.Id }, newFuelTypeDTO);
            }
            else
            {
                return BadRequest("Failed to add fuel type");
            }
        }

        [HttpPut("UpdateFuelType")]
        [ProducesResponseType(typeof(FuelTypeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateFuelType(FuelTypeDTO updatedFuelTypeDTO)
        {
            if (updatedFuelTypeDTO?.Id == null || !IsValidFuelType(updatedFuelTypeDTO))
                return BadRequest("Invalid Fuel Type Data");

            ClsFuelType fuelType = new ClsFuelType(updatedFuelTypeDTO, ClsFuelType.enMood.Update);

            if (fuelType.Save())
            {
                return Ok(updatedFuelTypeDTO);
            }
            else
            {
                return BadRequest("Failed to update fuel type");
            }
        }

        [HttpDelete("DeleteFuelType/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteFuelType(int id)
        {
            if (ClsFuelType.Delete(id))
            {
                return Ok($"Fuel Type with ID {id} deleted successfully.");
            }
            return NotFound($"Fuel Type with ID {id} not found.");
        }

        [HttpGet("GetFuelType/{id}")]
        [ProducesResponseType(typeof(FuelTypeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFuelTypeById(int id)
        {
            ClsFuelType? fuelType = ClsFuelType.GetById(id);
            if (fuelType == null)
                return NotFound($"Fuel Type with ID {id} not found.");

            return Ok(fuelType.DTO);
        }

        [HttpGet("GetAllFuelTypes")]
        [ProducesResponseType(typeof(List<FuelTypeDTO>), StatusCodes.Status200OK)]
        public IActionResult GetAllFuelTypes()
        {
            List<ClsFuelType> fuelTypes = ClsFuelType.GetAllFuelTypes();
            List<FuelTypeDTO> dtos = new List<FuelTypeDTO>();
            foreach (var ft in fuelTypes)
                dtos.Add(ft.DTO);

            return Ok(dtos);
        }
    }
}
