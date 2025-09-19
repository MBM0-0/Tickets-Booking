using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Repositories
{
    public class UserRepositorie : IUserRepositorie
    {
        private readonly ApplicationDbContext _context;
        public UserRepositorie(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>>GetAllAsync()
        {
           return await _context.Users.OrderBy(n => n.FullName).ToListAsync();
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);

        }
        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }
        public async Task DeleteAsync(int id)
        {
            var entity = _context.Users.Find(id);
            _context.Users.Remove(entity);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
