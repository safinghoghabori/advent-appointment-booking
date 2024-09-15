using advent_appointment_booking.Models;
using advent_appointment_booking.Services;
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

        // Create appointment (Trucking Company only)
        [HttpPost("create")]
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

        // Update appointment (Trucking Company only)
        [HttpPut("update/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointment(int appointmentId, [FromBody] Appointment appointment)
        {
            try
            {
                var result = await _appointmentService.UpdateAppointment(appointmentId, appointment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Get single appointment (Both Trucking Company and Terminal)
        [HttpGet("get/{appointmentId}")]
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

        // Get all appointments (Both Trucking Company and Terminal)
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAppointments()
        {
            var result = await _appointmentService.GetAppointments();
            return Ok(result);
        }

        // Delete appointment (Trucking Company only)
        [HttpDelete("delete/{appointmentId}")]
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

        // Cancel appointment (Terminal only)
        [HttpPut("cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            try
            {
                var result = await _appointmentService.CancelAppointment(appointmentId);
                return Ok(result);
            }
            catch (Exception ex) {
                return NotFound(new { message = ex.Message});
            }
        }
    }
}
