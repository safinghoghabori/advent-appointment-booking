using advent_appointment_booking.Database;
using advent_appointment_booking.DTOs;
using Microsoft.EntityFrameworkCore;

namespace advent_appointment_booking.Services
{
    public class TerminalsService: ITerminalsService
    {
        private readonly ApplicationDbContext _databaseContext;

        public TerminalsService(ApplicationDbContext databaseContext) { 
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<TerminalsDTO>> GetTerminalsAsync()
        {
            var data = await _databaseContext.Terminals.ToListAsync();

            if(data != null)
            {
                return data.Select(terminal => new TerminalsDTO { TerminalId = terminal.TerminalId, PortName =  terminal.PortName, TerminalName = terminal.TerminalName });    
            }
            else
            {
                throw new Exception("No terminals available.");
            }
        }
    }
}
