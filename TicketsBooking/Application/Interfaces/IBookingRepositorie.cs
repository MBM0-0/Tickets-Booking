using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Interfaces
{
    public interface IBookingRepositorie
    {
        public Task<List<Booking>> GetAllAsync();
        public Task<Booking> GetByIdAsync(int id);
        public Task AddAsync(Booking entity);
        public Task DeleteAsync(int id);
        public Task SaveChangesAsync();
    }
}
