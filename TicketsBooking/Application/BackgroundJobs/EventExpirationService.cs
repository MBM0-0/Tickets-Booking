using Microsoft.Extensions.DependencyInjection;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Infrastructure.Repositories;

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
                var eventrange = scope.ServiceProvider.GetRequiredService<IEventRepositorie>();
                var expiredevents = await eventrange.CancelAsync();

                foreach (var expire in expiredevents)
                { expire.IsEnded = true; }

                await eventrange.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);

            }
        }
    }
}
