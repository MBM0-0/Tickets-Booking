namespace TicketsBooking.Domain.Entities
{
    public class Event
    {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Location { get; set; }
    public int Capacity { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public bool IsEnded { get; set; }
    public ICollection<Booking> Bookings { get; set; }
    }
}
