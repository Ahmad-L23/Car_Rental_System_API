namespace CarRentalDataAccessLayer.DTOs
{
    public class MaintenanceReadDTO
    {
        public int MaintenanceID { get; set; }
        public int VehicleID { get; set; }
        public string? VehicleMake { get; set; }
        public string? VehicleModel { get; set; }
        public string? PlateNumber { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }
    }

    public class MaintenanceCreateDTO
    {
        public int VehicleID { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }
    }
}
