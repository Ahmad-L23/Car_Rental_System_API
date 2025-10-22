using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using University_DataAccess;
using CarRentalDataAccessLayer.DTOs;

namespace CarRentalDataAccessLayer
{
    public static class ClsMaintenanceData
    {
        private const string _baseSelectQuery = @"
            SELECT m.MaintenanceID, m.VehicleID, v.Make AS VehicleMake, v.Model AS VehicleModel, v.PlateNumber,
                   m.MaintenanceDate, m.Description, m.Cost
            FROM maintenance m
            INNER JOIN vehicle v ON m.VehicleID = v.VehicleID";

        public static int AddMaintenance(MaintenanceCreateDTO dto)
        {
            using var connection = new MySqlConnection(clsDataAccessSettings.ConnectionString);
            connection.Open();
            var query = @"INSERT INTO maintenance (VehicleID, MaintenanceDate, Description, Cost)
                          VALUES (@VehicleID, @MaintenanceDate, @Description, @Cost);
                          SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@VehicleID", dto.VehicleID);
            cmd.Parameters.AddWithValue("@MaintenanceDate", dto.MaintenanceDate);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@Cost", dto.Cost);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static bool UpdateMaintenance(int maintenanceID, MaintenanceCreateDTO dto)
        {
            using var connection = new MySqlConnection(clsDataAccessSettings.ConnectionString);
            connection.Open();
            var query = @"UPDATE maintenance 
                          SET VehicleID=@VehicleID, MaintenanceDate=@MaintenanceDate, Description=@Description, Cost=@Cost
                          WHERE MaintenanceID=@MaintenanceID";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@MaintenanceID", maintenanceID);
            cmd.Parameters.AddWithValue("@VehicleID", dto.VehicleID);
            cmd.Parameters.AddWithValue("@MaintenanceDate", dto.MaintenanceDate);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@Cost", dto.Cost);

            return cmd.ExecuteNonQuery() > 0;
        }

        private static MaintenanceReadDTO MapMaintenance(IDataReader reader)
        {
            return new MaintenanceReadDTO
            {
                MaintenanceID = Convert.ToInt32(reader["MaintenanceID"]),
                VehicleID = Convert.ToInt32(reader["VehicleID"]),
                VehicleMake = reader["VehicleMake"].ToString() ?? "",
                VehicleModel = reader["VehicleModel"].ToString() ?? "",
                PlateNumber = reader["PlateNumber"].ToString() ?? "",
                MaintenanceDate = Convert.ToDateTime(reader["MaintenanceDate"]),
                Description = reader["Description"].ToString() ?? "",
                Cost = Convert.ToDecimal(reader["Cost"])
            };
        }

        public static MaintenanceReadDTO? GetMaintenanceById(int maintenanceID)
        {
            using var connection = new MySqlConnection(clsDataAccessSettings.ConnectionString);
            connection.Open();
            var query = _baseSelectQuery + " WHERE m.MaintenanceID=@MaintenanceID";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@MaintenanceID", maintenanceID);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
                return MapMaintenance(reader);
            return null;
        }

        public static List<MaintenanceReadDTO> GetAllMaintenance()
        {
            var list = new List<MaintenanceReadDTO>();
            using var connection = new MySqlConnection(clsDataAccessSettings.ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand(_baseSelectQuery, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(MapMaintenance(reader));
            return list;
        }

        public static List<MaintenanceReadDTO> GetMaintenanceByDate(DateTime date)
        {
            var list = new List<MaintenanceReadDTO>();
            using var connection = new MySqlConnection(clsDataAccessSettings.ConnectionString);
            connection.Open();
            var query = _baseSelectQuery + " WHERE m.MaintenanceDate=@MaintenanceDate";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@MaintenanceDate", date);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(MapMaintenance(reader));
            return list;
        }

        public static List<MaintenanceReadDTO> GetMaintenanceByVehicle(int vehicleID)
        {
            var list = new List<MaintenanceReadDTO>();
            using var connection = new MySqlConnection(clsDataAccessSettings.ConnectionString);
            connection.Open();
            var query = _baseSelectQuery + " WHERE m.VehicleID=@VehicleID";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@VehicleID", vehicleID);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(MapMaintenance(reader));
            return list;
        }
    }
}
