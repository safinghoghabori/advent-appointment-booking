using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace advent_appointment_booking.Models
{
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }

        [ForeignKey("TruckingCompany")]  
        public int TrCompanyId { get; set; }

        public string DriverName { get; set; }
        public string PlateNumber { get; set; }
        public string PhoneNumber { get; set; }

        public TruckingCompany? TruckingCompany { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
