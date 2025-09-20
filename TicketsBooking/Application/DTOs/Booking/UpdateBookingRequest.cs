namespace TicketsBooking.Application.DTOs.Booking
{
    public class UpdateBookingRequest
    {
        public int id { get; set; }
        public int SeatBooked { get; set; } 
        public DateTime BookingDate { get; set; }
        public bool IsCancelld { get; set; }
    }
}
