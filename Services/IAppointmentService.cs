using advent_appointment_booking.DTOs;
using advent_appointment_booking.Models;

namespace advent_appointment_booking.Services
{
    public interface IAppointmentService
    {
        Task<CreateAppointmentDTO> CreateAppointment(Appointment appointment);
        Task<string> UpdateAppointment(int appointmentId, Appointment updatedAppointment);
        Task<object> GetAppointment(int appointmentId);
        Task<IEnumerable<CreateAppointmentDTO>> GetAppointments(string userId, string role);
        Task<string> DeleteAppointment(int appointmentId);
        Task<string> CancelAppointment(int appointmentId);
        Task<string> ApproveAppointment(int appointmentId);
        Task<List<string>> GetAvailableTimeSlots(int trCompanyId, DateOnly date);
        Task<string> UpdateAppointmentDateTime(int appointmentId, UpdateAppointmentDateTimeDto updatedAppointment);
    }
}
