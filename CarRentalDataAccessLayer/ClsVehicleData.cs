using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using University_DataAccess;

namespace CarRentalDataAccessLayer
{
    public class VehicleDTO
    {
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

        public VehicleDTO(
            int? vehicleID,
            string make,
            string model,
            int year,
            double mileage,
            string plateNumber,
            int carCategoryID,
            decimal rentalPricePerDay,
            bool isAvailableForRent,
            int fuelTypeID,
            string? categoryName = null,
            string? fuelTypeName = null)
        {
            VehicleID = vehicleID;
            Make = make;
            Model = model;
            Year = year;
            Mileage = mileage;
            PlateNumber = plateNumber;
            CarCategoryID = carCategoryID;
            RentalPricePerDay = rentalPricePerDay;
            IsAvailableForRent = isAvailableForRent;
            FuelTypeID = fuelTypeID;
            CategoryName = categoryName;
            FuelTypeName = fuelTypeName;
        }
    }

    public class ClsVehicleData
    {
        private static readonly string _connectionString = clsDataAccessSettings.ConnectionString;

        public static int AddNewVehicle(VehicleDTO dto)
        {
            using var conn = new MySqlConnection(_connectionString);
            string query = @"
                INSERT INTO vehicle 
                (Make, Model, Year, Mileage, PlateNumber, CarCategoryID, RentalPricePerDay, IsAvailableForRent, FuelTypeID)
                VALUES (@Make, @Model, @Year, @Mileage, @PlateNumber, @CarCategoryID, @RentalPricePerDay, @IsAvailableForRent, @FuelTypeID);
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Make", dto.Make);
            cmd.Parameters.AddWithValue("@Model", dto.Model);
            cmd.Parameters.AddWithValue("@Year", dto.Year);
            cmd.Parameters.AddWithValue("@Mileage", dto.Mileage);
            cmd.Parameters.AddWithValue("@PlateNumber", dto.PlateNumber);
            cmd.Parameters.AddWithValue("@CarCategoryID", dto.CarCategoryID);
            cmd.Parameters.AddWithValue("@RentalPricePerDay", dto.RentalPricePerDay);
            cmd.Parameters.AddWithValue("@IsAvailableForRent", dto.IsAvailableForRent);
            cmd.Parameters.AddWithValue("@FuelTypeID", dto.FuelTypeID);

            conn.Open();
            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public static bool UpdateVehicle(VehicleDTO dto)
        {
            using var conn = new MySqlConnection(_connectionString);
            string query = @"
                UPDATE vehicle
                SET Make=@Make, Model=@Model, Year=@Year, Mileage=@Mileage, PlateNumber=@PlateNumber, 
                    CarCategoryID=@CarCategoryID, RentalPricePerDay=@RentalPricePerDay, 
                    IsAvailableForRent=@IsAvailableForRent, FuelTypeID=@FuelTypeID
                WHERE VehicleID=@VehicleID";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@VehicleID", dto.VehicleID);
            cmd.Parameters.AddWithValue("@Make", dto.Make);
            cmd.Parameters.AddWithValue("@Model", dto.Model);
            cmd.Parameters.AddWithValue("@Year", dto.Year);
            cmd.Parameters.AddWithValue("@Mileage", dto.Mileage);
            cmd.Parameters.AddWithValue("@PlateNumber", dto.PlateNumber);
            cmd.Parameters.AddWithValue("@CarCategoryID", dto.CarCategoryID);
            cmd.Parameters.AddWithValue("@RentalPricePerDay", dto.RentalPricePerDay);
            cmd.Parameters.AddWithValue("@IsAvailableForRent", dto.IsAvailableForRent);
            cmd.Parameters.AddWithValue("@FuelTypeID", dto.FuelTypeID);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool Delete(int vehicleID)
        {
            using var conn = new MySqlConnection(_connectionString);
            string query = "DELETE FROM vehicle WHERE VehicleID=@VehicleID";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@VehicleID", vehicleID);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public static VehicleDTO? GetById(int vehicleID)
        {
            string query = @"
                SELECT v.*, vc.CategoryName, ft.FuelType
                FROM vehicle v
                INNER JOIN vehiclecategories vc ON v.CarCategoryID = vc.CategoryID
                INNER JOIN fueltypes ft ON v.FuelTypeID = ft.ID
                WHERE v.VehicleID=@VehicleID";

            return FetchSingleVehicle(query, new MySqlParameter("@VehicleID", vehicleID));
        }

        public static List<VehicleDTO> GetAllVehicles()
        {
            string query = @"
                SELECT v.*, vc.CategoryName, ft.FuelType
                FROM vehicle v
                INNER JOIN vehiclecategories vc ON v.CarCategoryID = vc.CategoryID
                INNER JOIN fueltypes ft ON v.FuelTypeID = ft.ID";
            return FetchVehicles(query);
        }

        public static List<VehicleDTO> GetAvailableVehicles()
        {
            string query = @"
                SELECT v.*, vc.CategoryName, ft.FuelType
                FROM vehicle v
                INNER JOIN vehiclecategories vc ON v.CarCategoryID = vc.CategoryID
                INNER JOIN fueltypes ft ON v.FuelTypeID = ft.ID
                WHERE v.IsAvailableForRent=1";
            return FetchVehicles(query);
        }

        public static List<VehicleDTO> GetVehiclesByRentalPrice(decimal rentalPrice)
        {
            string query = @"
                SELECT v.*, vc.CategoryName, ft.FuelType
                FROM vehicle v
                INNER JOIN vehiclecategories vc ON v.CarCategoryID = vc.CategoryID
                INNER JOIN fueltypes ft ON v.FuelTypeID = ft.ID
                WHERE v.RentalPricePerDay=@RentalPrice";
            return FetchVehicles(query, new MySqlParameter("@RentalPrice", rentalPrice));
        }

        public static List<VehicleDTO> GetVehiclesByYearAndCategory(int year, int categoryID)
        {
            string query = @"
                SELECT v.*, vc.CategoryName, ft.FuelType
                FROM vehicle v
                INNER JOIN vehiclecategories vc ON v.CarCategoryID = vc.CategoryID
                INNER JOIN fueltypes ft ON v.FuelTypeID = ft.ID
                WHERE v.Year=@Year AND v.CarCategoryID=@CategoryID";
            return FetchVehicles(query,
                new MySqlParameter("@Year", year),
                new MySqlParameter("@CategoryID", categoryID));
        }

        public static List<VehicleDTO> GetVehiclesByModel(string model)
        {
            string query = @"
                SELECT v.*, vc.CategoryName, ft.FuelType
                FROM vehicle v
                INNER JOIN vehiclecategories vc ON v.CarCategoryID = vc.CategoryID
                INNER JOIN fueltypes ft ON v.FuelTypeID = ft.ID
                WHERE v.Model LIKE @Model";
            return FetchVehicles(query, new MySqlParameter("@Model", $"%{model}%"));
        }

        public static List<VehicleDTO> GetVehiclesByCategory(int categoryID)
        {
            string query = @"
                SELECT v.*, vc.CategoryName, ft.FuelType
                FROM vehicle v
                INNER JOIN vehiclecategories vc ON v.CarCategoryID = vc.CategoryID
                INNER JOIN fueltypes ft ON v.FuelTypeID = ft.ID
                WHERE v.CarCategoryID=@CategoryID";
            return FetchVehicles(query, new MySqlParameter("@CategoryID", categoryID));
        }

        public static List<VehicleDTO> GetVehiclesByFuelType(int fuelTypeID)
        {
            string query = @"
                SELECT v.*, vc.CategoryName, ft.FuelType
                FROM vehicle v
                INNER JOIN vehiclecategories vc ON v.CarCategoryID = vc.CategoryID
                INNER JOIN fueltypes ft ON v.FuelTypeID = ft.ID
                WHERE v.FuelTypeID=@FuelTypeID";
            return FetchVehicles(query, new MySqlParameter("@FuelTypeID", fuelTypeID));
        }

        public static int GetVehicleCount()
        {
            using var conn = new MySqlConnection(_connectionString);
            string query = "SELECT COUNT(*) FROM vehicle";
            using var cmd = new MySqlCommand(query, conn);
            conn.Open();
            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        private static VehicleDTO? FetchSingleVehicle(string query, params MySqlParameter[] parameters)
        {
            using var conn = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand(query, conn);
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
                return MapReaderToVehicle(reader);
            return null;
        }

        private static List<VehicleDTO> FetchVehicles(string query, params MySqlParameter[] parameters)
        {
            var list = new List<VehicleDTO>();
            using var conn = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand(query, conn);
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(MapReaderToVehicle(reader));
            return list;
        }

        private static VehicleDTO MapReaderToVehicle(MySqlDataReader reader)
        {
            return new VehicleDTO(
                Convert.ToInt32(reader["VehicleID"]),
                reader["Make"]?.ToString() ?? "",
                reader["Model"]?.ToString() ?? "",
                Convert.ToInt32(reader["Year"]),
                Convert.ToDouble(reader["Mileage"]),
                reader["PlateNumber"]?.ToString() ?? "",
                Convert.ToInt32(reader["CarCategoryID"]),
                Convert.ToDecimal(reader["RentalPricePerDay"]),
                Convert.ToBoolean(reader["IsAvailableForRent"]),
                Convert.ToInt32(reader["FuelTypeID"]),
                reader["CategoryName"]?.ToString(),
                reader["FuelType"]?.ToString()
            );
        }
    }
}
