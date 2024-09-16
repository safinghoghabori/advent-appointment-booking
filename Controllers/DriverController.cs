using advent_appointment_booking.Enums;
using advent_appointment_booking.Models;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using advent_appointment_booking.DTOs;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        // Create a new driver
        [HttpPost]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> CreateDriver([FromBody] Driver driver)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors });
            }

            try
            {
                var result = await _driverService.CreateDriver(driver);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Get details of a driver by ID
        [HttpGet("{driverId}")]
        public async Task<IActionResult> GetDriver(int driverId)
        {
            try {
                var driver = await _driverService.GetDriver(driverId);
                return Ok(driver);
            }
            catch(Exception ex) { 
                return NotFound(new { message = ex.Message });
            }
        }


        // Get all drivers
        [HttpGet]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetAllDrivers();
            return Ok(drivers);
        }


        // Update a driver
        [HttpPut("{driverId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> UpdateDriver(int driverId, [FromBody] Driver driver)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors });
            }

            try
            {
                var result = await _driverService.UpdateDriver(driverId, driver);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Delete a driver
        [HttpDelete("{driverId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> DeleteDriver(int driverId)
        {
            try
            {
                var result = await _driverService.DeleteDriver(driverId);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
