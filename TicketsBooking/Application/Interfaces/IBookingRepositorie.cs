using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Interfaces
{
    public interface IBookingRepositorie
    {
        public Task<List<Booking>> GetAllAsync();
        public Task<Booking> GetByIdAsync(int id);
        public Task<bool> GetDuplicateDataAsync(int userId, int eventId);
        public Task AddAsync(Booking entity);
        public Task<int> GetBookedSeatsAsync(int id);
        public Task SaveChangesAsync();
    }
}
