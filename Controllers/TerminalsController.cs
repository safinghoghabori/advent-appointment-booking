using advent_appointment_booking.Enums;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminalsController : ControllerBase
    {
        private readonly ITerminalsService _terminalsService;

        public TerminalsController(ITerminalsService terminalsService) 
        { 
            _terminalsService = terminalsService;
        }

        [HttpGet]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> GetTerminals()
        {
            try
            {
                var result = await _terminalsService.GetTerminalsAsync();
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(new { message = ex.Message });    
            }
        }
    }
}
