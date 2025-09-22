namespace TicketsBooking.Application.DTOs.Booking
{
    public class CreateBookingRequest
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public int SeatBooked { get; set; } 
    }
}
