using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using TicketsBooking.Application.DTOs;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Repositories
{
    public class EventRepositorie :IEventRepositorie
    {
        private readonly ApplicationDbContext _context;
       // private readonly IMapper _mapper;
        public EventRepositorie(ApplicationDbContext context )
        {  _context = context;}
    public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Events.OrderBy(d => d.Date).ThenBy(t=>t.Time).ToListAsync();
        }
    public async Task AddAsync(Event CreateEventRequest)
        {
            //var Entity = new Event
            //{
            //    Name = CreateEventRequest.Name,
            //    Description = CreateEventRequest.Description,
            //    Location = CreateEventRequest.Location,
            //    capacity = CreateEventRequest.capacity,
            //    Date = CreateEventRequest.Date,
            //    Time = CreateEventRequest.Time
            //};
            await _context.Events.AddAsync(CreateEventRequest);
            await _context.SaveChangesAsync();
        }
    }
}
