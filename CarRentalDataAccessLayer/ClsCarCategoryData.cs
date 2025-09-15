using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using University_DataAccess;

namespace CarRentalDataAccessLayer
{
    public class CarCategoryDTO
    {
        public int? Id { get; set; }
        public string CategoryName { get; set; }

        public CarCategoryDTO(int? id, string categoryName)
        {
            Id = id;
            CategoryName = categoryName;
        }
    }

    public class ClsCarCategoryData
    {
        private static readonly string _connectionString = clsDataAccessSettings.ConnectionString;

        public static int AddNewCategory(CarCategoryDTO dto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO vehiclecategories (CategoryName)
                    VALUES (@CategoryName);
                    SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryName", dto.CategoryName);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        public static bool UpdateCategory(CarCategoryDTO dto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE vehiclecategories
                    SET CategoryName = @CategoryName
                    WHERE CategoryID = @Id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", dto.Id);
                    cmd.Parameters.AddWithValue("@CategoryName", dto.CategoryName);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool Delete(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM vehiclecategories WHERE CategoryID = @Id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static CarCategoryDTO? GetById(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM vehiclecategories WHERE CategoryID = @Id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CarCategoryDTO(
                                Convert.ToInt32(reader["CategoryID"]),
                                reader["CategoryName"]?.ToString() ?? ""
                            );
                        }

                        return null;
                    }
                }
            }
        }

        public static List<CarCategoryDTO> GetAllCategories()
        {
            List<CarCategoryDTO> categories = new();

            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM vehiclecategories";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new CarCategoryDTO(
                                Convert.ToInt32(reader["CategoryID"]),
                                reader["CategoryName"]?.ToString() ?? ""
                            ));
                        }
                    }
                }
            }

            return categories;
        }
    }
}
