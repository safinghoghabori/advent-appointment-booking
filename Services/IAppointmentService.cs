using advent_appointment_booking.Models;

namespace advent_appointment_booking.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAppointment(Appointment appointment);
        Task<Appointment> UpdateAppointment(int appointmentId, Appointment updatedAppointment);
        Task<Appointment> GetAppointment(int appointmentId);
        Task<IEnumerable<object>> GetAppointments();
        Task<string> DeleteAppointment(int appointmentId);
        Task<string> CancelAppointment(int appointmentId);

    }
}
