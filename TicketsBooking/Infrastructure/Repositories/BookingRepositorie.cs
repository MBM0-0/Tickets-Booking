using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.DTOs.Booking;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Repositories
{
    public class BookingRepositorie : IBookingRepositorie
    {
        private readonly ApplicationDbContext _dbcontext;

        public BookingRepositorie(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<List<Booking>> GetAllAsync()
        {
            return await _dbcontext.Bookings.Include(e => e.Event).Include(u => u.User).Where(o => o.IsCancelled == false).ToListAsync();
        }
        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _dbcontext.Bookings.Include(b => b.User).Include(b => b.Event).FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<bool> BookingExistsAsync(int eventId, int userId)
        {
            return await _dbcontext.Bookings.AnyAsync(b => b.EventId == eventId && b.UserId == userId);
        }
        public async Task AddAsync(Booking entity)
        {
            await _dbcontext.Bookings.AddAsync(entity);
        }
        public async Task<int> GetBookedSeatsAsync(int id)
        {
            var count = await _dbcontext.Bookings.Where(x => x.EventId == id).Where(x => x.IsCancelled == false).SumAsync(x => x.SeatBooked);

            return count;
        }
        public async Task SaveChangesAsync()
        {
            await _dbcontext.SaveChangesAsync();
        }
    }
}
