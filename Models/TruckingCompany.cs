using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace advent_appointment_booking.Models
{
    public class TruckingCompany
    {
        [Key]  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrCompanyId { get; set; }

        [Required]  
        [MaxLength(200)]  
        public string CompanyName { get; set; }

        [MaxLength(50)]
        public string GstNo { get; set; }

        [MaxLength(50)]
        public string TransportLicNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
