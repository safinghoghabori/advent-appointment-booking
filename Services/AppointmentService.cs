using advent_appointment_booking.Database;
using advent_appointment_booking.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace advent_appointment_booking.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _databaseContext;

        public AppointmentService(ApplicationDbContext context)
        {
            _databaseContext = context;
        }

        // Create Appointment (Trucking Company only)
        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            var truckingCompany = await _databaseContext.TruckingCompanies.FindAsync(appointment.TrCompanyId);
            if (truckingCompany == null)
                throw new Exception("Invalid Trucking Company.");

            var terminal = await _databaseContext.Terminals.FindAsync(appointment.TerminalId);
            if (terminal == null)
                throw new Exception("Invalid Terminal.");

            // Generate custom gate code: first two letters of Trucking Company + first three digits of Terminal ID
            string gateCode = $"{truckingCompany.TrCompanyName.Substring(0, 2).ToUpper()}{terminal.TerminalId.ToString().PadLeft(3, '0')}";
            appointment.GateCode = gateCode;

            appointment.AppointmentCreated = DateTime.UtcNow;
            appointment.AppointmentLastModified = DateTime.UtcNow;
            appointment.AppointmentValidThrough = appointment.AppointmentCreated.AddDays(2);
            appointment.AppointmentStatus = "Scheduled";

            await _databaseContext.Appointments.AddAsync(appointment);
            await _databaseContext.SaveChangesAsync();

            return appointment;
        }

        // Update Appointment (Trucking Company only)
        public async Task<Appointment> UpdateAppointment(int appointmentId, Appointment updatedAppointment)
        {
            var appointment = await _databaseContext.Appointments.FindAsync(appointmentId);
            if (appointment == null || appointment.TrCompanyId != updatedAppointment.TrCompanyId)
                throw new Exception("Unauthorized or invalid appointment.");

            appointment.MoveType = updatedAppointment.MoveType;
            appointment.ContainerNumber = updatedAppointment.ContainerNumber;
            appointment.SizeType = updatedAppointment.SizeType;
            appointment.Line = updatedAppointment.Line;
            appointment.ChassisNo = updatedAppointment.ChassisNo;
            appointment.AppointmentLastModified = DateTime.UtcNow;
            appointment.AppointmentValidThrough = appointment.AppointmentLastModified.AddDays(2);

            _databaseContext.Appointments.Update(appointment);
            await _databaseContext.SaveChangesAsync();

            return appointment;
        }

        // Get Appointment (Accessible to both Trucking Company and Terminal)
        public async Task<Appointment> GetAppointment(int appointmentId)
        {
            var appointment = await _databaseContext.Appointments
                .Include(a => a.Terminal)
                .Include(a => a.TruckingCompany)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            return appointment;
        }

        // Get All Appointments (Accessible to both Trucking Company and Terminal)
        public async Task<IEnumerable<object>> GetAppointments()
        {
            return await _databaseContext.Appointments
                .Select(a => new
                {
                    a.AppointmentId,
                    a.ContainerNumber,
                    a.SizeType,
                    a.AppointmentCreated,
                    a.AppointmentStatus,
                    a.AppointmentValidThrough
                })
                .ToListAsync();
        }

        // Delete Appointment (Trucking Company only)
        public async Task<string> DeleteAppointment(int appointmentId)
        {
            var appointment = await _databaseContext.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                throw new Exception("Appointment not found.");

            _databaseContext.Appointments.Remove(appointment);
            await _databaseContext.SaveChangesAsync();

            return "Appointment deleted successfully.";
        }

        // Cancel Appointment (Terminal only)
        public async Task<string> CancelAppointment(int appointmentId)
        {
            var appointment = await _databaseContext.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                throw new Exception("Appointment not found.");

            appointment.AppointmentStatus = "Canceled";

            _databaseContext.Appointments.Update(appointment);
            await _databaseContext.SaveChangesAsync();

            return "Appointment canceled successfully.";
        }
    }
}
