using advent_appointment_booking.Models;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("truckingcompany")]
        public async Task<IActionResult> RegisterTruckingCompany([FromBody] TruckingCompany model)
        {
            // Validate incoming model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _registrationService.RegisterTruckingCompnay(model);
                return Ok(new { message = "Trucking company registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("terminal")]
        public async Task<IActionResult> RegisterTerminal([FromBody] Terminal model)
        {
            // Validate incoming model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _registrationService.RegisterTerminal(model);
                return Ok(new { message = "Terminal registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
