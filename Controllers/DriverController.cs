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
                if (ex.Message.Contains("Trucking company doesn't exist"))
                {
                    return NotFound(new { message = ex.Message });
                }
                else if (ex.Message.Contains("Driver with the same plate number already exists") ||
                         ex.Message.Contains("Driver with the same phone number already exists"))
                {
                    return Conflict(new { message = ex.Message });
                }

                return BadRequest(new { message = ex.Message });
            }
        }

        // Get details of a driver by ID
        [HttpGet("{driverId}")]
        public async Task<IActionResult> GetDriver(int driverId)
        {
            var driver = await _driverService.GetDriver(driverId);

            if (driver == null)
            {
                return NotFound(new { message = "Driver not found" });
            }

            // Map the Driver entity to DriverDTO
            var driverDto = new DriverDTO
            {
                DriverId = driver.DriverId,
                TrCompanyId = driver.TrCompanyId,
                DriverName = driver.DriverName,
                PlateNo = driver.PlateNo,
                PhoneNumber = driver.PhoneNumber,
                TruckingCompany = new TruckingCompanyDTO
                {
                    TrCompanyId = driver.TruckingCompany.TrCompanyId,
                    TrCompanyName = driver.TruckingCompany.TrCompanyName,
                    GstNo = driver.TruckingCompany.GstNo,
                    TransportLicNo = driver.TruckingCompany.TransportLicNo,
                    Email = driver.TruckingCompany.Email,
                    CreatedAt = driver.TruckingCompany.CreatedAt,
                    UpdatedAt = driver.TruckingCompany.UpdatedAt
                }
            };

            return Ok(driverDto);
        }


        // Get all drivers
        [HttpGet]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetAllDrivers();

            // Map each Driver entity to DriverDTO to remove circular references
            var driverDtos = drivers.Select(driver => new DriverDTO
            {
                DriverId = driver.DriverId,
                TrCompanyId = driver.TrCompanyId,
                DriverName = driver.DriverName,
                PlateNo = driver.PlateNo,
                PhoneNumber = driver.PhoneNumber,
                TruckingCompany = new TruckingCompanyDTO
                {
                    TrCompanyId = driver.TruckingCompany.TrCompanyId,
                    TrCompanyName = driver.TruckingCompany.TrCompanyName,
                    GstNo = driver.TruckingCompany.GstNo,
                    TransportLicNo = driver.TruckingCompany.TransportLicNo,
                    Email = driver.TruckingCompany.Email,
                    CreatedAt = driver.TruckingCompany.CreatedAt,
                    UpdatedAt = driver.TruckingCompany.UpdatedAt
                }
            }).ToList();

            return Ok(driverDtos);
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
