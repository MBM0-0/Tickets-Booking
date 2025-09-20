using TicketsBooking.Application.DTOs.Event;
using TicketsBooking.Application.DTOs.User;

namespace TicketsBooking.Application.DTOs.Booking
{
    public class BookingResponse
    {
        public int id { get; set; }
        public int EventId { get; set; }
        public EventResponse Event { get; set; }
        public UserResponse User { get; set; }
        public int SeatBooked { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public bool IsCancelld { get; set; }
    }
}
