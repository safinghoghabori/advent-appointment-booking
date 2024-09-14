using advent_appointment_booking.Models;
using Microsoft.EntityFrameworkCore;

namespace advent_appointment_booking.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TruckingCompany> TruckingCompanies { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
    }
}
