namespace TicketsBooking.Application.DTOs
{
    public class UpdateEventRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public int capacity { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
    }
}
