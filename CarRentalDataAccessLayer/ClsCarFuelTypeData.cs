using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using University_DataAccess;

namespace CarRentalDataAccessLayer
{
    public class FuelTypeDTO
    {
        public int? Id { get; set; }
        public string FuelType { get; set; }

        public FuelTypeDTO(int? id, string fuelType)
        {
            Id = id;
            FuelType = fuelType;
        }
    }

    public class ClsCarFuelTypeData
    {
        private static readonly string _connectionString = clsDataAccessSettings.ConnectionString;

        public static int AddNewFuelType(FuelTypeDTO dto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO fueltypes (FuelType)
                    VALUES (@FuelType);
                    SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FuelType", dto.FuelType);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        public static bool UpdateFuelType(FuelTypeDTO dto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE fueltypes
                    SET FuelType = @FuelType
                    WHERE ID = @Id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", dto.Id);
                    cmd.Parameters.AddWithValue("@FuelType", dto.FuelType);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool Delete(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM fueltypes WHERE ID = @Id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static FuelTypeDTO? GetById(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM fueltypes WHERE ID = @Id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new FuelTypeDTO(
                                Convert.ToInt32(reader["ID"]),
                                reader["FuelType"]?.ToString() ?? ""
                            );
                        }

                        return null;
                    }
                }
            }
        }

        public static List<FuelTypeDTO> GetAllFuelTypes()
        {
            List<FuelTypeDTO> list = new List<FuelTypeDTO>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM fueltypes";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new FuelTypeDTO(
                                Convert.ToInt32(reader["ID"]),
                                reader["FuelType"]?.ToString() ?? ""
                            ));
                        }
                    }
                }
            }

            return list;
        }
    }
}
