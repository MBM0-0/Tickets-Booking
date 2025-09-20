
using Mapster;
using MapsterMapper;
using TicketsBooking.Application.DTOs.Event;
using TicketsBooking.Application.Exceptions;
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
            if (events == null || !events.Any())
                throw new NotFoundException("There is No Events to Show");

            return events.Adapt<List<EventResponse>>();
        }

        public async Task<EventResponse> GetEventByIdAsync(int id)
        {
            var entity = await _eventRepositorie.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("There is No Event Found With This Id");
            return entity.Adapt<EventResponse>();
        }

        public async Task<EventResponse> CreateEventAsync(CreateEventRequest dto)
        {
            var entity = dto.Adapt<Event>();
            if (entity.capacity <= 0)
                throw new ValidationException("You Can't Have an Event With No Seat Capacity");
            if (entity.DateTime <= DateTime.UtcNow)
                throw new ValidationException("You Can't Create Events in the Past");

            await _eventRepositorie.AddAsync(entity);
            await _eventRepositorie.SaveChangesAsync();
            return entity.Adapt<EventResponse>();
        }

        public async Task<UpdateEventRequest> UpdateEventAsync(UpdateEventRequest dto)
        {
            var entity = await _eventRepositorie.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new NotFoundException("There is No Event Found With This Id");
            if (dto.capacity <= 0)
                throw new ValidationException("You Can't Have an Event With No Seat Capacity");
            if (dto.DateTime <= DateTime.UtcNow)
                throw new ValidationException("You Can't Create Events in the Past");


            dto.Adapt(entity);
            await _eventRepositorie.SaveChangesAsync();
            return dto;
        }

        public async Task DeleteEventAsync(int id)
        {
            var entity = await _eventRepositorie.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("There is No Event Found With This Id");

            await _eventRepositorie.DeleteAsync(entity);
            await _eventRepositorie.SaveChangesAsync();
        }
    }
}
