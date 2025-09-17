using Microsoft.EntityFrameworkCore;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ) : base (options)
        { }
        public DbSet<Event> Events { get; set; }
    }
}
