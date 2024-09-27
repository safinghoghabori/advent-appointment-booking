using advent_appointment_booking.Database;
using advent_appointment_booking.Enums;
using advent_appointment_booking.Helpers;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace advent_appointment_booking.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _databaseContext;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(ApplicationDbContext databaseContext, JwtTokenGenerator jwtTokenGenerator)
        {
            _databaseContext = databaseContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
    
        public async Task<object> LoginAsync(string email, string password, string userType)
        {
            if (userType == UserType.TruckingCompany)
            {
                var trCompany = await _databaseContext.TruckingCompanies
                    .Where(tc => tc.Email == email && tc.Password == password).FirstOrDefaultAsync();

                if (trCompany == null)
                {
                    throw new Exception("Invalid credentials.");
                }

                var token = _jwtTokenGenerator.GenerateToken(email, userType, trCompany.TrCompanyId);

                var result = new
                {
                    trCompany.TrCompanyName,
                    trCompany.Email,
                    trCompany.TransportLicNo,
                    trCompany.GstNo,
                    trCompany.CreatedAt,
                    trCompany.UpdatedAt
                };

                return new {data = result, token}; 
            }
            else if (userType == UserType.Terminal)
            {
                var terminal = await _databaseContext.Terminals
                    .Where(t => t.Email == email && t.Password == password)
                    .FirstOrDefaultAsync();

                if (terminal == null)
                {
                    throw new Exception("Invalid credentials.");
                }

                var token = _jwtTokenGenerator.GenerateToken(email, userType, terminal.TerminalId);

                var result = new
                {
                    terminal.PortName,
                    terminal.Email,
                    terminal.Address,
                    terminal.City,
                    terminal.State,
                    terminal.Country,
                    terminal.CreatedAt,
                    terminal.UpdatedAt
                };
                return new { data = terminal, token }; 
            }

            throw new Exception("Invalid user type.");
        }
    }
}
