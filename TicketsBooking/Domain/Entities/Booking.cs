namespace TicketsBooking.Domain.Entities
{
    public class Booking
    {
        public int id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SeatBooked { get; set; } // per user
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public bool IsCancelld { get; set; }
    }
}
