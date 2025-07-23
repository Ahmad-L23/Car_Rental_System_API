using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using University_DataAccess;

namespace CarRentalDataAccessLayer
{
    public class CustomerDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ContactInformation { get; set; }
        public string DriverLicenseNumber { get; set; }

        public CustomerDTO(int? id, string name, string contactInformation, string driverLicenseNumber)
        {
            Id = id;
            Name = name;
            ContactInformation = contactInformation;
            DriverLicenseNumber = driverLicenseNumber;
        }
    }

    public class ClsCustomerData
    {
        private static readonly string _connectionString = clsDataAccessSettings.ConnectionString;

        public static int AddNewCustomer(CustomerDTO CDTO)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Customer (Name, ContactInformation, DriverLicenseNumber)
                    VALUES (@Name, @ContactInfo, @License);
                    SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", CDTO.Name);
                    cmd.Parameters.AddWithValue("@ContactInfo", CDTO.ContactInformation);
                    cmd.Parameters.AddWithValue("@License", CDTO.DriverLicenseNumber);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        public static bool Update(CustomerDTO dto)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Customer
                    SET Name = @Name, ContactInformation = @ContactInfo, DriverLicenseNumber = @License
                    WHERE Id = @Id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", dto.Id);
                    cmd.Parameters.AddWithValue("@Name", dto.Name);
                    cmd.Parameters.AddWithValue("@ContactInfo", dto.ContactInformation);
                    cmd.Parameters.AddWithValue("@License", dto.DriverLicenseNumber);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool Delete(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM Customer WHERE Id = @Id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static CustomerDTO? GetById(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Customer WHERE Id = @Id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CustomerDTO(
                                Convert.ToInt32(reader["Id"]),
                                reader["Name"]?.ToString() ?? "",
                                reader["ContactInformation"]?.ToString() ?? "",
                                reader["DriverLicenseNumber"]?.ToString() ?? ""
                            );
                        }

                        return null;
                    }
                }
            }
        }

        public static List<CustomerDTO> GetAllCustomers()
        {
            List<CustomerDTO> customers = new();

            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Customer";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new CustomerDTO(
                                Convert.ToInt32(reader["Id"]),
                                reader["Name"]?.ToString() ?? "",
                                reader["ContactInformation"]?.ToString() ?? "",
                                reader["DriverLicenseNumber"]?.ToString() ?? ""
                            ));
                        }
                    }
                }
            }

            return customers;
        }

        public static List<CustomerDTO> GetByName(string name)
        {
            List<CustomerDTO> customers = new();

            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Customer WHERE Name LIKE @Name";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", $"%{name}%");

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new CustomerDTO(
                                Convert.ToInt32(reader["Id"]),
                                reader["Name"]?.ToString() ?? "",
                                reader["ContactInformation"]?.ToString() ?? "",
                                reader["DriverLicenseNumber"]?.ToString() ?? ""
                            ));
                        }
                    }
                }
            }

            return customers;
        }
    }
}
