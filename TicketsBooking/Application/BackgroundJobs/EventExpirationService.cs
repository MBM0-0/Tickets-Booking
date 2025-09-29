using Microsoft.EntityFrameworkCore;
using TicketsBooking.Infrastructure;

namespace TicketsBooking.Application.BackgroundJobs
{
    public class EventExpirationService :BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public EventExpirationService(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync (CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var now = DateTime.UtcNow;

                var expiredEvents = await dbcontext.Events.Where(e => e.StartsAt < now && e.IsEnded == false).ToListAsync(cancellationToken);

                if (expiredEvents.Any())
                {
                    foreach (var ev in expiredEvents)
                    {
                        ev.IsEnded = true;

                        var bookings = await dbcontext.Bookings.Where(b => b.EventId == ev.Id && b.IsCancelled == false).ToListAsync(cancellationToken);

                        foreach (var booking in bookings)
                        {
                            booking.IsCancelled = true;
                        }
                    }

                    await dbcontext.SaveChangesAsync(cancellationToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
    }
}
