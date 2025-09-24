
using Mapster;
using TicketsBooking.Application.DTOs.Event;
using TicketsBooking.Application.Exceptions;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;

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
                throw new NotFoundException("There is No Events to Show.");

            return events.Adapt<List<EventResponse>>();
        }

        public async Task<EventResponse> GetEventByIdAsync(int id)
        {
            var entity = await _eventRepositorie.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("There is No Event Found With This Id.");
            return entity.Adapt<EventResponse>();
        }

        public async Task<EventResponse> CreateEventAsync(CreateEventRequest dto)
        {
            var entity = dto.Adapt<Event>();
            var checkduplicate = await _eventRepositorie.GetDuplicateDataAsync(dto.Name);
            if (checkduplicate)
                throw new ValidationException("There Is Alrady an active event with this name");

            if (entity.Capacity < 5)
                throw new ValidationException("Seat Capacity Can't be Less Than 5.");
            if (entity.StartsAt <= DateTime.UtcNow)
                throw new ValidationException("events can't be created in the past.");
            if ((entity.EndsAt - entity.StartsAt).TotalHours <= 1)
                throw new ValidationException("Event duration must be at least one hour.");

            entity.CreatedAt = DateTime.UtcNow;
            await _eventRepositorie.AddAsync(entity);
            await _eventRepositorie.SaveChangesAsync();
            return entity.Adapt<EventResponse>();
        }

        public async Task<UpdateEventRequest> UpdateEventAsync(UpdateEventRequest dto)
        {
            var entity = await _eventRepositorie.GetByIdAsync(dto.Id);
            if (entity == null || entity.IsEnded)
                throw new NotFoundException("Event not found or has already ended.");

            if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != entity.Name)
            {
                var checkDuplicate = await _eventRepositorie.GetDuplicateDataAsync(dto.Name);
                if (checkDuplicate)
                    throw new ValidationException("There is already an active event with this name.");
                entity.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Description))
                entity.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.Location))
                entity.Location = dto.Location;

            if (dto.Capacity > 0)
            {
                if (dto.Capacity < entity.Capacity)
                    throw new ValidationException("The number of seats cannot be decreased from the original capacity.");
                entity.Capacity = dto.Capacity;
            }

            if (dto.StartsAt != default(DateTime))
            {
                if (dto.StartsAt <= DateTime.UtcNow)
                    throw new ValidationException("events can't be created in the past.");
                entity.StartsAt = dto.StartsAt;
            }

            if (dto.EndsAt != default(DateTime))
            {
                if ((dto.EndsAt - (dto.StartsAt != default(DateTime) ? dto.StartsAt : entity.StartsAt)).TotalHours < 1)
                    throw new ValidationException("Event duration must be at least one hour.");
                entity.EndsAt = dto.EndsAt;
            }
            entity.UpdatedAt = DateTime.UtcNow;
            var result = new UpdateEventRequest
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Location = entity.Location,
                Capacity = entity.Capacity,
                StartsAt = entity.StartsAt,
                EndsAt = entity.EndsAt
            };
            await _eventRepositorie.SaveChangesAsync();
            return result;
        }

        public async Task DeleteEventAsync(int id)
        {
            var entity = await _eventRepositorie.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("There is No Event Found With This Id.");

            await _eventRepositorie.DeleteAsync(entity);
            await _eventRepositorie.SaveChangesAsync();
        }
    }
}
