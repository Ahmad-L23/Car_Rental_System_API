using System;
using System.Collections.Generic;
using CarRentalDataAccessLayer;
using CarRentalDataAccessLayer.DTOs;

namespace CarRentalAPIBusinessLayer
{
    public class ClsMaintenance
    {
        public enum enMode { Add = 0, Update = 1 }

        public int MaintenanceID { get; set; }
        public int VehicleID { get; set; }
        public string? VehicleMake { get; set; }
        public string? VehicleModel { get; set; }
        public string? PlateNumber { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public enMode Mode { get; set; } = enMode.Add;

        public MaintenanceCreateDTO CreateDTO => new MaintenanceCreateDTO
        {
            VehicleID = VehicleID,
            MaintenanceDate = MaintenanceDate,
            Description = Description,
            Cost = Cost
        };

        public ClsMaintenance(MaintenanceReadDTO dto)
        {
            MaintenanceID = dto.MaintenanceID;
            VehicleID = dto.VehicleID;
            VehicleMake = dto.VehicleMake;
            VehicleModel = dto.VehicleModel;
            PlateNumber = dto.PlateNumber;
            MaintenanceDate = dto.MaintenanceDate;
            Description = dto.Description;
            Cost = dto.Cost;
            Mode = enMode.Update;
        }

        public ClsMaintenance() { }

        private bool _AddMaintenance()
        {
            MaintenanceID = ClsMaintenanceData.AddMaintenance(CreateDTO);
            if (MaintenanceID != -1) Mode = enMode.Update;
            return MaintenanceID != -1;
        }

        private bool _UpdateMaintenance()
        {
            return ClsMaintenanceData.UpdateMaintenance(MaintenanceID, CreateDTO);
        }

        public bool Save()
        {
            return Mode switch
            {
                enMode.Add => _AddMaintenance(),
                enMode.Update => _UpdateMaintenance(),
                _ => false
            };
        }

        public static ClsMaintenance? GetById(int maintenanceID)
        {
            var dto = ClsMaintenanceData.GetMaintenanceById(maintenanceID);
            return dto != null ? new ClsMaintenance(dto) : null;
        }

        public static List<ClsMaintenance> GetAllMaintenance()
        {
            var list = new List<ClsMaintenance>();
            var dtos = ClsMaintenanceData.GetAllMaintenance();
            foreach (var dto in dtos)
                list.Add(new ClsMaintenance(dto));
            return list;
        }

        public static List<ClsMaintenance> GetMaintenanceByDate(DateTime date)
        {
            var list = new List<ClsMaintenance>();
            var dtos = ClsMaintenanceData.GetMaintenanceByDate(date);
            foreach (var dto in dtos)
                list.Add(new ClsMaintenance(dto));
            return list;
        }

        public static List<ClsMaintenance> GetMaintenanceByVehicle(int vehicleID)
        {
            var list = new List<ClsMaintenance>();
            var dtos = ClsMaintenanceData.GetMaintenanceByVehicle(vehicleID);
            foreach (var dto in dtos)
                list.Add(new ClsMaintenance(dto));
            return list;
        }
    }
}
