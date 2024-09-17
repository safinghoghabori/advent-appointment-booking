namespace advent_appointment_booking.DTOs
{
    public class DriverDTO
    {
        public int DriverId { get; set; }
        public int TrCompanyId { get; set; }
        public string DriverName { get; set; }
        public string PlateNo { get; set; }
        public string PhoneNumber { get; set; }
        public TruckingCompanyDTO TruckingCompany { get; set; }
    }

    public class TruckingCompanyDTO
    {
        public int TrCompanyId { get; set; }
        public string TrCompanyName { get; set; }
        public string GstNo { get; set; }
        public string TransportLicNo { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}