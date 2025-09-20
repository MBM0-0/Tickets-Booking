using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using TicketsBooking.Application.DTOs;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Repositories
{
    public class EventRepositorie :IEventRepositorie
    {
        private readonly ApplicationDbContext _context;
        public EventRepositorie(ApplicationDbContext context )
        {
            _context = context;
        }

    public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Events.OrderBy(d => d.DateTime).ToListAsync();
        }
    public async Task<Event> GetByIdAsync( int id )
        {
            return await _context.Events.FindAsync(id);
        }
        public async Task AddAsync(Event entity)
        {
            await _context.Events.AddAsync(entity);
        }
        public async Task DeleteAsync (Event entity)
    {
            _context.Events.Remove(entity);

    }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
