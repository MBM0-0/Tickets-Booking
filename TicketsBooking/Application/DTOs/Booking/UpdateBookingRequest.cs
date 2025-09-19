namespace TicketsBooking.Application.DTOs.Booking
{
    public class UpdateBookingRequest
    {
        public int id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public int SeatBooked { get; set; } // per user
        public DateTime BookingDate { get; set; }
        public bool IsCancelld { get; set; }
    }
}
