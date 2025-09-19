namespace TicketsBooking.Application.DTOs.Event
{
    public class EventAvailability
    {
            public int id { get; set; }
            public int seatCapacity { get; set; }
            public int seatsBooked { get; set; }
            public int seatsAvailable { get; set; } // per user
    }
}
