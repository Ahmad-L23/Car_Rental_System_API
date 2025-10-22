using CarRentalAPIBusinessLayer;
using CarRentalDataAccessLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CarRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<MaintenanceReadDTO>> GetAll()
        {
            var list = ClsMaintenance.GetAllMaintenance();
            return Ok(list.ConvertAll(m => new MaintenanceReadDTO
            {
                MaintenanceID = m.MaintenanceID,
                VehicleID = m.VehicleID,
                VehicleMake = m.VehicleMake ?? "",
                VehicleModel = m.VehicleModel ?? "",
                PlateNumber = m.PlateNumber ?? "",
                MaintenanceDate = m.MaintenanceDate,
                Description = m.Description,
                Cost = m.Cost
            }));
        }

        [HttpGet("{id}")]
        public ActionResult<MaintenanceReadDTO> GetById(int id)
        {
            var maintenance = ClsMaintenance.GetById(id);
            if (maintenance == null) return NotFound();

            return Ok(new MaintenanceReadDTO
            {
                MaintenanceID = maintenance.MaintenanceID,
                VehicleID = maintenance.VehicleID,
                VehicleMake = maintenance.VehicleMake ?? "",
                VehicleModel = maintenance.VehicleModel ?? "",
                PlateNumber = maintenance.PlateNumber ?? "",
                MaintenanceDate = maintenance.MaintenanceDate,
                Description = maintenance.Description,
                Cost = maintenance.Cost
            });
        }

        [HttpGet("date/{date}")]
        public ActionResult<List<MaintenanceReadDTO>> GetByDate(DateTime date)
        {
            var list = ClsMaintenance.GetMaintenanceByDate(date);
            return Ok(list.ConvertAll(m => new MaintenanceReadDTO
            {
                MaintenanceID = m.MaintenanceID,
                VehicleID = m.VehicleID,
                VehicleMake = m.VehicleMake ?? "",
                VehicleModel = m.VehicleModel ?? "",
                PlateNumber = m.PlateNumber ?? "",
                MaintenanceDate = m.MaintenanceDate,
                Description = m.Description,
                Cost = m.Cost
            }));
        }

        [HttpGet("vehicle/{vehicleID}")]
        public ActionResult<List<MaintenanceReadDTO>> GetByVehicle(int vehicleID)
        {
            var list = ClsMaintenance.GetMaintenanceByVehicle(vehicleID);
            return Ok(list.ConvertAll(m => new MaintenanceReadDTO
            {
                MaintenanceID = m.MaintenanceID,
                VehicleID = m.VehicleID,
                VehicleMake = m.VehicleMake ?? "",
                VehicleModel = m.VehicleModel ?? "",
                PlateNumber = m.PlateNumber ?? "",
                MaintenanceDate = m.MaintenanceDate,
                Description = m.Description,
                Cost = m.Cost
            }));
        }

        [HttpPost]
        public ActionResult Add([FromBody] MaintenanceCreateDTO dto)
        {
            var maintenance = new ClsMaintenance
            {
                VehicleID = dto.VehicleID,
                MaintenanceDate = dto.MaintenanceDate,
                Description = dto.Description,
                Cost = dto.Cost,
                Mode = ClsMaintenance.enMode.Add
            };

            bool success = maintenance.Save();
            if (success) return Ok(new { maintenance.MaintenanceID });

            return BadRequest("Failed to add maintenance record.");
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] MaintenanceCreateDTO dto)
        {
            var existing = ClsMaintenance.GetById(id);
            if (existing == null) return NotFound();

            existing.VehicleID = dto.VehicleID;
            existing.MaintenanceDate = dto.MaintenanceDate;
            existing.Description = dto.Description;
            existing.Cost = dto.Cost;
            existing.Mode = ClsMaintenance.enMode.Update;

            bool success = existing.Save();
            if (success) return Ok();

            return BadRequest("Failed to update maintenance record.");
        }
    }
}
