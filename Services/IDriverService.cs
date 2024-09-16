using advent_appointment_booking.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace advent_appointment_booking.Services
{
    public interface IDriverService
    {
        Task<Driver> CreateDriver(Driver driver);
        Task<Driver> GetDriver(int driverId);
        Task<IEnumerable<Driver>> GetAllDrivers();
        Task<string> UpdateDriver(int driverId, Driver driver);
        Task<string> DeleteDriver(int driverId);
    }
}
