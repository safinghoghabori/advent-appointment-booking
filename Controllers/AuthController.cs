using advent_appointment_booking.DTOs;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /*
            POST /api/auth?userType=TruckingCompany
            {
                "email": "example@company.com",
                "password": "password123"
            }
        */
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login login, [FromQuery] string userType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
