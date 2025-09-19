
using Mapster;
using MapsterMapper;
using TicketsBooking.Application.DTOs;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Repositories;
using TicketsBooking.Migrations;

namespace TicketsBooking.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepositorie _eventRepositorie;
        public EventService(IEventRepositorie eventRepositorie)
        {
            _eventRepositorie = eventRepositorie;
        }

        public async Task<List<EventResponse>> GetAllEventsAsync()
        {
            var events = await _eventRepositorie.GetAllAsync();
            // empty
            return events.Adapt<List<EventResponse>>();
        }
        public async Task<EventResponse> GetEventByIdAsync(int id)
        {
            var events = await _eventRepositorie.GetByIdAsync(id);
            return events.Adapt<EventResponse>();
        }
        public async Task<EventResponse> CreateEventAsync(CreateEventRequest dto)
        {
            var entity = dto.Adapt<Event>();
            await _eventRepositorie.AddAsync(entity);
            // cant exapt time in the past
            // seats cant be less than 0
            await _eventRepositorie.SaveChangesAsync();
            return entity.Adapt<EventResponse>();
        }
        public async Task<UpdateEventRequest> UpdateEventAsync(UpdateEventRequest dto)
        {
            var existingEvent = await _eventRepositorie.GetByIdAsync(dto.Id);
            dto.Adapt(existingEvent);
            await _eventRepositorie.SaveChangesAsync();
            return dto;
        }
        public async Task DeleteEventAsync(int id)
        {
            // if null or empty
            var entity = _eventRepositorie.DeleteAsync(id);
            await _eventRepositorie.SaveChangesAsync();
        }
    }
}
