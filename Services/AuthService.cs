using advent_appointment_booking.Database;
using Microsoft.EntityFrameworkCore;

namespace advent_appointment_booking.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _databaseContext;

        public AuthService(ApplicationDbContext context)
        {
            _databaseContext = context;
        }
    
        public async Task<object> LoginAsync(string email, string password, string userType)
        {
            if (userType == "TruckingCompany")
            {
                var company = await _databaseContext.TruckingCompanies
                    .Where(tc => tc.Email == email && tc.Password == password)
                    .Select(company => new
                    {
                        company.TrCompanyName,
                        company.Email,
                        company.TransportLicNo,
                        company.GstNo,
                        company.CreatedAt,
                        company.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (company == null)
                {
                    throw new Exception("Invalid credentials for Trucking Company.");
                }

                return company; 
            }
            else if (userType == "Terminal")
            {
                var terminal = await _databaseContext.Terminals
                    .Where(t => t.Email == email && t.Password == password)
                    .Select(tr => new
                    {
                        tr.PortName,
                        tr.Email,
                        tr.Address,
                        tr.City,
                        tr.State,
                        tr.Country,
                        tr.CreatedAt,
                        tr.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (terminal == null)
                {
                    throw new Exception("Invalid credentials for Terminal.");
                }

                return terminal; 
            }

            throw new Exception("Invalid user type.");
        }
    }
}
