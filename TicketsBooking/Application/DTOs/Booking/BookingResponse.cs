using TicketsBooking.Application.DTOs.Event;
using TicketsBooking.Application.DTOs.User;

namespace TicketsBooking.Application.DTOs.Booking
{
    public class BookingResponse
    {
        public int id { get; set; }
        public int EventId { get; set; }
        public EventResponse Event { get; set; } //= new EventResponse
        public UserResponse User { get; set; } //= new UserResponse();
        public int SeatBooked { get; set; } // per user
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public bool IsCancelld { get; set; }
    }
}
