using Mapster;
using TicketsBooking.Application.DTOs.Booking;
using TicketsBooking.Application.Exceptions;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Services
{
    public class BookingService :IBookingService
    {
    private readonly IBookingRepositorie _bookingRepositorie;
    private readonly IEventRepositorie _eventRepositorie;
    private readonly IUserRepositorie _userRepositorie;
    public BookingService (IBookingRepositorie bookingRepositorie , IEventRepositorie eventRepositorie , IUserRepositorie userRepositorie)
        {
            _bookingRepositorie = bookingRepositorie;
            _eventRepositorie = eventRepositorie;
            _userRepositorie = userRepositorie;
        }
        public async Task<List<BookingResponse>> GetAllBookingAsync()
        {
            var entitys = await _bookingRepositorie.GetAllAsync();
            if (entitys == null || !entitys.Any())
                throw new NotFoundException("There is No Bookings to Show.");

            return entitys.Adapt<List<BookingResponse>>();
        }

        public async Task<BookingResponse> GetBookingByIdAsync(int id)
        {
            var entity = await _bookingRepositorie.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("There is No Booking Found Whith This Id.");
            return entity.Adapt<BookingResponse>();
        }

        public async Task<BookingSeatAvailability> GetSeatAvailabilityAsync(int id)
        {
            var eventEntity = await _eventRepositorie.GetByIdAsync(id);
            if (eventEntity == null)
                throw new NotFoundException("There is No Event Found Whith This Id.");
            var capacity = eventEntity.Capacity;
            var bookedSeats = await _bookingRepositorie.GetBookedSeatsAsync(id);
            var freeSeats = capacity - bookedSeats;
           var result = new BookingSeatAvailability
            {
                SeatCapacity = capacity,
                SeatsBooked = bookedSeats,
                SeatsAvailable = freeSeats
            };
            return result;
        }

        public async Task<CreateBookingRequest> AddBookingAsync(CreateBookingRequest dto)
        {
            var CheckEventId = await _eventRepositorie.GetByIdAsync(dto.EventId);
            if (CheckEventId == null)
                throw new NotFoundException("There is No Event Found Whith This Id.");
            var CheckUserId = await _userRepositorie.GetByIdAsync(dto.UserId);
            if (CheckUserId == null)
                throw new NotFoundException("There is No User Found Whith This Id.");
            var exists = await _bookingRepositorie.BookingExistsAsync(dto.EventId, dto.UserId);
            if (exists)
                throw new ValidationException("The User Already Has a Booking for this Event.");

            var entity = dto.Adapt<Booking>();
            if (entity.SeatBooked <= 0 || entity.SeatBooked > 4)
                throw new ValidationException("You Have To Book Seats and it Can't Be More Than 4.");
            if (CheckEventId.StartsAt <= DateTime.UtcNow)
                throw new ValidationException("You can't book an event that has already started or ended.");

            var bookedSeats = await GetSeatAvailabilityAsync(dto.EventId);
            if (bookedSeats.SeatsAvailable < dto.SeatBooked)
                throw new ValidationException($"Event Have {bookedSeats.SeatsAvailable} Seats Availabil.");

            entity.CreatedAt = DateTime.UtcNow;
            await _bookingRepositorie.AddAsync(entity);
            await _bookingRepositorie.SaveChangesAsync();
            var response = entity.Adapt<CreateBookingRequest>();
            return response;
        }

        public async Task<UpdateBookingRequest> UpdateBookingAsync(UpdateBookingRequest dto)
        {
            var entity = await _bookingRepositorie.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new NotFoundException("There is No Booking Found Whith This Id.");
            if (dto.SeatBooked <= 0 || dto.SeatBooked > 4)
                throw new ValidationException("You Have To Book Seats and it Can't Be More Than 4.");
            // if event is done or started you cant update it
            var bookedSeats = await GetSeatAvailabilityAsync(entity.EventId);
            if ((bookedSeats.SeatsAvailable + entity.SeatBooked) < dto.SeatBooked)
                throw new ValidationException($"Event Have {bookedSeats.SeatsAvailable} Seats Availabil.");

            entity.UpdatedAt = DateTime.UtcNow;
            dto.Adapt(entity);
            await _bookingRepositorie.SaveChangesAsync();
            return dto;
        }

        public async Task CancelBookingAsync(int id)
        {
           var entity = await _bookingRepositorie.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("There is No Booking Found Whith This Id.");
            if (entity.IsCancelled == true)
                throw new ValidationException("Booking is already Cancelle.");
            entity.CancelledAt = DateTime.UtcNow;
            entity.IsCancelled = true;
            await _bookingRepositorie.SaveChangesAsync();
        }
    }
}
