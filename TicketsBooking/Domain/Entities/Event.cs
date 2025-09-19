namespace TicketsBooking.Domain.Entities
{
    public class Event
    {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Location { get; set; }
    public int capacity { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }

    public ICollection<Booking> Bookings { get; set; }
    }
}
