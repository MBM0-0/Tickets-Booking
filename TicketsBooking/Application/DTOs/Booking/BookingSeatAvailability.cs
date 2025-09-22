namespace TicketsBooking.Application.DTOs.Booking
{
    public class BookingSeatAvailability
    {
        public int SeatCapacity { get; set; }
        public int SeatsBooked { get; set; }
        public int SeatsAvailable { get; set; }
    }
}
