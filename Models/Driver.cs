using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace advent_appointment_booking.Models
{
    public class Driver
    {
        [Key]  
        public int DriverId { get; set; }

        [ForeignKey("TruckingCompany")]  
        public int TrCompanyId { get; set; }

        [ForeignKey("Appointment")]  
        public int AppointmentId { get; set; }

        public string DriverName { get; set; }
        public string PlateNo { get; set; }
        public string PhoneNumber { get; set; }

        public TruckingCompany TruckingCompany { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
