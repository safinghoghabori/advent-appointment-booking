using advent_appointment_booking.DTOs;

namespace advent_appointment_booking.Services
{
    public interface ITerminalsService
    {
        Task<IEnumerable<TerminalsDTO>> GetTerminalsAsync();
    }
}
