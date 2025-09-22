using TicketsBooking.Application.DTOs.Event;
using TicketsBooking.Application.DTOs.User;

namespace TicketsBooking.Application.DTOs.Booking
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public EventResponse Event { get; set; }
        public UserResponse User { get; set; }
        public int SeatBooked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public bool IsCancelled { get; set; }
    }
}
