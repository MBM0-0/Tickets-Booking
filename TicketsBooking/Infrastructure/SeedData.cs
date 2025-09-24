using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure
{
    public class SeedData
    {
    public static async Task AddSeedDataAsync(ApplicationDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    Id = 1,
                    FullName = "Admin User",
                    Username = "admin",
                    Email = "admin@tickets.com",
                    Password = "123456Aa"
                },
                new User
                {
                    Id = 2,
                    FullName = "james hetfield",
                    Username = "Hetfield",
                    Email = "Hetfield@Metallica.com",
                    Password = "Metallica"
                }
            );

            await context.SaveChangesAsync();
        }
    }
}
}
