using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Interfaces
{
    public interface IUserRepositorie
    {
        public Task<List<User>> GetAllAsync();
        public Task<User> GetByIdAsync(int id);
        public Task AddAsync(User entity);
        public Task DeleteAsync(User entity);
        public Task<User?> GetByEmailAsync(string email);
        public Task<User?> GetByUsernameAsync(string username);
        public Task SaveChangesAsync();

    }
}
