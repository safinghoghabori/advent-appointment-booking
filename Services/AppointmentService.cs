using advent_appointment_booking.Database;
using advent_appointment_booking.DTOs;
using advent_appointment_booking.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<CreateAppointmentDTO> CreateAppointment(Appointment appointment)
        {
            var truckingCompany = await _databaseContext.TruckingCompanies.FindAsync(appointment.TrCompanyId);
            if (truckingCompany == null)
                throw new Exception("Invalid Trucking Company.");

            var terminal = await _databaseContext.Terminals.FindAsync(appointment.TerminalId);
            if (terminal == null)
                throw new Exception("Invalid Terminal.");

            var driver = await _databaseContext.Drivers.FindAsync(appointment.DriverId);
            if (driver == null)
                throw new Exception("Driver does not exists.");

            var isContainerAlreadyScheduled = await _databaseContext.Appointments.AnyAsync(a => a.ContainerNumber == appointment.ContainerNumber);
            if(isContainerAlreadyScheduled)
            {
                throw new Exception("Appointment for entered container number is already exists.");
            }

            // Generate custom gate code: first two letters of Trucking Company + first three digits of Terminal ID
            string gateCode = $"{truckingCompany.TrCompanyName.Substring(0, 2).ToUpper()}{terminal.TerminalId.ToString().PadLeft(3, '0')}";
            appointment.GateCode = gateCode;

            appointment.AppointmentCreated = DateTime.UtcNow;
            appointment.AppointmentLastModified = DateTime.UtcNow;
            appointment.AppointmentValidThrough = appointment.AppointmentCreated.AddDays(2);
            appointment.AppointmentStatus = "Scheduled";

            await _databaseContext.Appointments.AddAsync(appointment);
            await _databaseContext.SaveChangesAsync();

            return new CreateAppointmentDTO
            {
                TrCompanyName = truckingCompany.TrCompanyName,
                GstNo = truckingCompany.GstNo,
                TransportLicNo = truckingCompany.TransportLicNo,
                PortName = terminal.PortName,
                Address = terminal.Address,
                City = terminal.City,
                State = terminal.State,
                Country = terminal.Country,
                DriverName = driver.DriverName,
                PlateNo = driver.PlateNo,
                PhoneNumber = driver.PhoneNumber,
                MoveType = appointment.MoveType,
                ContainerNumber = appointment.ContainerNumber,
                SizeType = appointment.SizeType,
                Line = appointment.Line,
                ChassisNo = appointment.ChassisNo,
                AppointmentStatus = appointment.AppointmentStatus,
                AppointmentCreated = appointment.AppointmentCreated,
                AppointmentValidThrough = appointment.AppointmentValidThrough,
                AppointmentLastModified = appointment.AppointmentLastModified,
                GateCode = appointment.GateCode
            };
        }

        // Update Appointment (Trucking Company only)
        public async Task<string> UpdateAppointment(int appointmentId, Appointment updatedAppointment)
        {
            var appointment = await _databaseContext.Appointments.FindAsync(appointmentId);
            if (appointment == null || appointment.TrCompanyId != updatedAppointment.TrCompanyId)
                throw new Exception("Unauthorized or invalid appointment.");

            appointment.MoveType = updatedAppointment.MoveType;
            appointment.ContainerNumber = updatedAppointment.ContainerNumber;
            appointment.SizeType = updatedAppointment.SizeType;
            appointment.Line = updatedAppointment.Line;
            appointment.ChassisNo = updatedAppointment.ChassisNo;
            appointment.TerminalId = updatedAppointment.TerminalId;
            appointment.DriverId = updatedAppointment.DriverId;
            appointment.AppointmentLastModified = DateTime.UtcNow;

            _databaseContext.Appointments.Update(appointment);
            await _databaseContext.SaveChangesAsync();

            return "Appointment updated successfully.";
        }

        // Get Appointment (Accessible to both Trucking Company and Terminal)
        public async Task<object> GetAppointment(int appointmentId)
        {
            var appointment = await _databaseContext.Appointments
                .Select(a => new
                {
                    a.AppointmentId,
                    a.ContainerNumber,
                    a.SizeType,
                    a.Line,
                    a.ChassisNo,
                    a.GateCode,
                    a.AppointmentCreated,
                    a.AppointmentStatus,
                    a.AppointmentValidThrough,
                    a.TruckingCompany.TrCompanyName,
                    a.TruckingCompany.Email,
                    a.TruckingCompany.GstNo,
                    a.TruckingCompany.TransportLicNo,
                    a.Terminal.PortName,
                    a.Terminal.City,
                    a.Terminal.State,
                    a.Terminal.Country,
                    a.Terminal.Address,
                    a.Driver.DriverName,
                    a.Driver.PlateNo,
                    a.Driver.PhoneNumber
                })
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
                    a.Line,
                    a.ChassisNo,
                    a.GateCode,
                    a.AppointmentCreated,
                    a.AppointmentStatus,
                    a.AppointmentValidThrough,
                    a.TruckingCompany.TrCompanyName,
                    a.TruckingCompany.Email,
                    a.TruckingCompany.GstNo,
                    a.TruckingCompany.TransportLicNo,
                    a.Terminal.PortName,
                    a.Terminal.City,
                    a.Terminal.State,
                    a.Terminal.Country,
                    a.Terminal.Address,
                    a.Driver.DriverName,
                    a.Driver.PlateNo,
                    a.Driver.PhoneNumber
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
