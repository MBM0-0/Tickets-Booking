
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
            public async Task<EventResponse> CreateEventAsync(CreateEventRequest dto)
            {
            var entity = dto.Adapt<Event>();
                await _eventRepositorie.AddAsync(entity);
            // cant exapt time in the past
            // seats cant be less than 0
            return entity.Adapt<EventResponse>(); 
            }
        }
    }
