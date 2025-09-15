using CarRentalAPIBusinessLayer;
using CarRentalDataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private bool IsValidVehicle(VehicleDTO dto)
        {
            if (dto == null) return false;
            if (string.IsNullOrEmpty(dto.Make)) return false;
            if (string.IsNullOrEmpty(dto.Model)) return false;
            if (dto.Year <= 0) return false;
            if (dto.Mileage < 0) return false;
            if (string.IsNullOrEmpty(dto.PlateNumber)) return false;
            if (dto.CarCategoryID <= 0) return false;
            if (dto.RentalPricePerDay <= 0) return false;
            if (dto.FuelTypeID <= 0) return false;
            return true;
        }

        [HttpPost("AddVehicle", Name = "addNewVehicle")]
        [ProducesResponseType(typeof(VehicleDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddVehicle(VehicleDTO newVehicleDTO)
        {
            if (!IsValidVehicle(newVehicleDTO))
                return BadRequest("Invalid Vehicle Data");

            ClsVehicle vehicle = new ClsVehicle(newVehicleDTO);

            if (vehicle.Save())
            {
                newVehicleDTO.VehicleID = vehicle.VehicleID;
                return CreatedAtRoute("addNewVehicle", new { id = newVehicleDTO.VehicleID }, newVehicleDTO);
            }
            return BadRequest("Failed to add vehicle");
        }

        [HttpPut("UpdateVehicle")]
        [ProducesResponseType(typeof(VehicleDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVehicle(VehicleDTO updatedVehicleDTO)
        {
            if (updatedVehicleDTO?.VehicleID == null || !IsValidVehicle(updatedVehicleDTO))
                return BadRequest("Invalid Vehicle Data");

            ClsVehicle vehicle = new ClsVehicle(updatedVehicleDTO, ClsVehicle.enMood.Update);

            if (vehicle.Save())
            {
                return Ok(updatedVehicleDTO);
            }
            return BadRequest("Failed to update vehicle");
        }

        [HttpDelete("DeleteVehicle/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVehicle(int id)
        {
            if (ClsVehicle.Delete(id))
                return Ok($"Vehicle with ID {id} deleted successfully.");
            return NotFound($"Vehicle with ID {id} not found.");
        }

        [HttpGet("GetVehicle/{id}")]
        [ProducesResponseType(typeof(VehicleDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetVehicleById(int id)
        {
            ClsVehicle? vehicle = ClsVehicle.GetById(id);
            if (vehicle == null)
                return NotFound($"Vehicle with ID {id} not found.");
            return Ok(vehicle.DTO);
        }

        [HttpGet("GetAllVehicles")]
        [ProducesResponseType(typeof(List<VehicleDTO>), StatusCodes.Status200OK)]
        public IActionResult GetAllVehicles()
        {
            List<ClsVehicle> vehicles = ClsVehicle.GetAllVehicles();
            List<VehicleDTO> dtos = new List<VehicleDTO>();
            foreach (var v in vehicles)
                dtos.Add(v.DTO);
            return Ok(dtos);
        }

        [HttpGet("GetAvailableVehicles")]
        [ProducesResponseType(typeof(List<VehicleDTO>), StatusCodes.Status200OK)]
        public IActionResult GetAvailableVehicles()
        {
            List<ClsVehicle> vehicles = ClsVehicle.GetAvailableVehicles();
            List<VehicleDTO> dtos = new List<VehicleDTO>();
            foreach (var v in vehicles)
                dtos.Add(v.DTO);
            return Ok(dtos);
        }

        [HttpGet("GetVehiclesByRentalPrice/{price}")]
        [ProducesResponseType(typeof(List<VehicleDTO>), StatusCodes.Status200OK)]
        public IActionResult GetVehiclesByRentalPrice(decimal price)
        {
            List<ClsVehicle> vehicles = ClsVehicle.GetVehiclesByRentalPrice(price);
            List<VehicleDTO> dtos = new List<VehicleDTO>();
            foreach (var v in vehicles)
                dtos.Add(v.DTO);
            return Ok(dtos);
        }

        [HttpGet("GetVehiclesByYearAndCategory/{year}/{categoryID}")]
        [ProducesResponseType(typeof(List<VehicleDTO>), StatusCodes.Status200OK)]
        public IActionResult GetVehiclesByYearAndCategory(int year, int categoryID)
        {
            List<ClsVehicle> vehicles = ClsVehicle.GetVehiclesByYearAndCategory(year, categoryID);
            List<VehicleDTO> dtos = new List<VehicleDTO>();
            foreach (var v in vehicles)
                dtos.Add(v.DTO);
            return Ok(dtos);
        }

        [HttpGet("GetVehiclesByModel/{model}")]
        [ProducesResponseType(typeof(List<VehicleDTO>), StatusCodes.Status200OK)]
        public IActionResult GetVehiclesByModel(string model)
        {
            List<ClsVehicle> vehicles = ClsVehicle.GetVehiclesByModel(model);
            List<VehicleDTO> dtos = new List<VehicleDTO>();
            foreach (var v in vehicles)
                dtos.Add(v.DTO);
            return Ok(dtos);
        }

        [HttpGet("GetVehicleCount")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult GetVehicleCount()
        {
            return Ok(ClsVehicle.GetVehicleCount());
        }

        [HttpGet("GetVehiclesByCategory/{categoryID}")]
        [ProducesResponseType(typeof(List<VehicleDTO>), StatusCodes.Status200OK)]
        public IActionResult GetVehiclesByCategory(int categoryID)
        {
            List<ClsVehicle> vehicles = ClsVehicle.GetVehiclesByCategory(categoryID);
            List<VehicleDTO> dtos = new List<VehicleDTO>();
            foreach (var v in vehicles)
                dtos.Add(v.DTO);
            return Ok(dtos);
        }

        [HttpGet("GetVehiclesByFuelType/{fuelTypeID}")]
        [ProducesResponseType(typeof(List<VehicleDTO>), StatusCodes.Status200OK)]
        public IActionResult GetVehiclesByFuelType(int fuelTypeID)
        {
            List<ClsVehicle> vehicles = ClsVehicle.GetVehiclesByFuelType(fuelTypeID);
            List<VehicleDTO> dtos = new List<VehicleDTO>();
            foreach (var v in vehicles)
                dtos.Add(v.DTO);
            return Ok(dtos);
        }
    }
}
