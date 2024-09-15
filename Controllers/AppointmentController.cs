using advent_appointment_booking.Enums;
using advent_appointment_booking.Models;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("create")]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
        {
            try
            {
                var result = await _appointmentService.CreateAppointment(appointment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPut("update/{appointmentId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> UpdateAppointment(int appointmentId, [FromBody] Appointment appointment)
        {
            try
            {
                var result = await _appointmentService.UpdateAppointment(appointmentId, appointment);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get/{appointmentId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyOrTerminalRole)]
        public async Task<IActionResult> GetAppointment(int appointmentId)
        {
            try
            {
                var result = await _appointmentService.GetAppointment(appointmentId);
                return Ok(result);
            }
            catch (Exception ex) {
                return NotFound(new { message = ex.Message});
            }
        }

        [HttpGet("get-all")]
        [Authorize(Policy = Policy.RequireTruckingCompanyOrTerminalRole)]
        public async Task<IActionResult> GetAppointments()
        {
            var result = await _appointmentService.GetAppointments();
            return Ok(result);
        }

        [HttpDelete("delete/{appointmentId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> DeleteAppointment(int appointmentId)
        {
            try
            {
                var result = await _appointmentService.DeleteAppointment(appointmentId);
                return Ok(new { message = result });
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("cancel/{appointmentId}")]
        [Authorize(Policy = Policy.RequireTerminalRole)]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            try
            {
                var result = await _appointmentService.CancelAppointment(appointmentId);
                return Ok(new { message = result });
            }
            catch (Exception ex) {
                return NotFound(new { message = ex.Message});
            }
        }
    }
}
