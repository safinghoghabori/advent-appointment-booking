using advent_appointment_booking.DTOs;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login, [FromQuery] string userType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid login details" });
            }

            try
            {
                var result = await _authService.LoginAsync(login.Email, login.Password, userType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }

}
