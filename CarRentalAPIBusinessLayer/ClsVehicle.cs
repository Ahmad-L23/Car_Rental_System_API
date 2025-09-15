using CarRentalDataAccessLayer;
using System;
using System.Collections.Generic;

namespace CarRentalAPIBusinessLayer
{
    public class ClsVehicle
    {
        public enum enMood { Add = 0, Update = 1 }

        public int? VehicleID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double Mileage { get; set; }
        public string PlateNumber { get; set; }
        public int CarCategoryID { get; set; }
        public decimal RentalPricePerDay { get; set; }
        public bool IsAvailableForRent { get; set; }
        public int FuelTypeID { get; set; }
        public string? CategoryName { get; set; }
        public string? FuelTypeName { get; set; }
        public enMood Mood = enMood.Add;

        public VehicleDTO DTO => new VehicleDTO(
            VehicleID, Make, Model, Year, Mileage, PlateNumber,
            CarCategoryID, RentalPricePerDay, IsAvailableForRent, FuelTypeID,
            CategoryName, FuelTypeName
        );

        public ClsVehicle(VehicleDTO dto, enMood mood = enMood.Add)
        {
            VehicleID = dto.VehicleID;
            Make = dto.Make;
            Model = dto.Model;
            Year = dto.Year;
            Mileage = dto.Mileage;
            PlateNumber = dto.PlateNumber;
            CarCategoryID = dto.CarCategoryID;
            RentalPricePerDay = dto.RentalPricePerDay;
            IsAvailableForRent = dto.IsAvailableForRent;
            FuelTypeID = dto.FuelTypeID;
            CategoryName = dto.CategoryName;
            FuelTypeName = dto.FuelTypeName;
            Mood = mood;
        }

        private bool AddNewVehicle()
        {
            VehicleID = ClsVehicleData.AddNewVehicle(DTO);
            return VehicleID != -1;
        }

        private bool _UpdateVehicle() => ClsVehicleData.UpdateVehicle(DTO);

        public bool Save()
        {
            switch (Mood)
            {
                case enMood.Add: return AddNewVehicle() && (Mood = enMood.Update) == enMood.Update;
                case enMood.Update: return _UpdateVehicle();
                default: return false;
            }
        }

        public static bool Delete(int vehicleID) => ClsVehicleData.Delete(vehicleID);

        public static ClsVehicle? GetById(int vehicleID)
        {
            var dto = ClsVehicleData.GetById(vehicleID);
            return dto != null ? new ClsVehicle(dto, enMood.Update) : null;
        }

        public static List<ClsVehicle> GetAllVehicles()
        {
            var list = new List<ClsVehicle>();
            foreach (var dto in ClsVehicleData.GetAllVehicles())
                list.Add(new ClsVehicle(dto, enMood.Update));
            return list;
        }

        public static List<ClsVehicle> GetAvailableVehicles()
        {
            var list = new List<ClsVehicle>();
            foreach (var dto in ClsVehicleData.GetAvailableVehicles())
                list.Add(new ClsVehicle(dto, enMood.Update));
            return list;
        }

        public static List<ClsVehicle> GetVehiclesByRentalPrice(decimal price)
        {
            var list = new List<ClsVehicle>();
            foreach (var dto in ClsVehicleData.GetVehiclesByRentalPrice(price))
                list.Add(new ClsVehicle(dto, enMood.Update));
            return list;
        }

        public static List<ClsVehicle> GetVehiclesByYearAndCategory(int year, int categoryID)
        {
            var list = new List<ClsVehicle>();
            foreach (var dto in ClsVehicleData.GetVehiclesByYearAndCategory(year, categoryID))
                list.Add(new ClsVehicle(dto, enMood.Update));
            return list;
        }

        public static List<ClsVehicle> GetVehiclesByModel(string model)
        {
            var list = new List<ClsVehicle>();
            foreach (var dto in ClsVehicleData.GetVehiclesByModel(model))
                list.Add(new ClsVehicle(dto, enMood.Update));
            return list;
        }

        public static List<ClsVehicle> GetVehiclesByCategory(int categoryID)
        {
            var list = new List<ClsVehicle>();
            foreach (var dto in ClsVehicleData.GetVehiclesByCategory(categoryID))
                list.Add(new ClsVehicle(dto, enMood.Update));
            return list;
        }

        public static List<ClsVehicle> GetVehiclesByFuelType(int fuelTypeID)
        {
            var list = new List<ClsVehicle>();
            foreach (var dto in ClsVehicleData.GetVehiclesByFuelType(fuelTypeID))
                list.Add(new ClsVehicle(dto, enMood.Update));
            return list;
        }

        public static int GetVehicleCount() => ClsVehicleData.GetVehicleCount();
    }
}
