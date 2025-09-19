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
            return await _dbcontext.Bookings.Include(e => e.Event).Include(u => u.User).OrderByDescending(o => o.IsCancelld).ToListAsync();
        }
        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _dbcontext.Bookings.Include(b => b.User).Include(b => b.Event).FirstOrDefaultAsync(b => b.id == id);
        }
        public async Task AddAsync(Booking entity)
        {
            await _dbcontext.Bookings.AddAsync(entity);
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _dbcontext.Bookings.FindAsync(id);
            _dbcontext.Bookings.Remove(entity);
        }
        public async Task SaveChangesAsync()
        {
            await _dbcontext.SaveChangesAsync();
        }
    }
}
