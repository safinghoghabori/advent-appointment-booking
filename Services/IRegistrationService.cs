﻿using advent_appointment_booking.Models;

namespace advent_appointment_booking.Services
{
    public interface IRegistrationService
    {
        Task<bool> RegisterTruckingCompnay(TruckingCompany truckingCompany);
        Task<bool> RegisterTerminal(Terminal terminal);
        Task<bool> IsEmailExists(string email);
    }
}
