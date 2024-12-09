namespace advent_appointment_booking.DTOs
{
    public class UpdateAppointmentDateTimeDto
    {
        public DateOnly AppointmentDate {  get; set; }
        public string TimeSlot { get; set; }
    }
}
