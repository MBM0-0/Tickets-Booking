using Mapster;
using TicketsBooking.Application.DTOs.Booking;
using TicketsBooking.Application.DTOs.Event;
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
            var entity = await _bookingRepositorie.GetAllAsync();
            return entity.Adapt<List<BookingResponse>>();
        }
        public async Task<BookingResponse> GetBookingById(int id)
        {
            var entity = await _bookingRepositorie.GetByIdAsync(id);
            return entity.Adapt<BookingResponse>();
        }
        public async Task<CreateBookingRequest> AddBookingAsync(CreateBookingRequest dto)
        {
            var entity = dto.Adapt<Booking>();
            await _bookingRepositorie.AddAsync(entity);
            await _bookingRepositorie.SaveChangesAsync();
            var response = entity.Adapt<CreateBookingRequest>();
            return response;
        }
        public async Task<UpdateBookingRequest> UpdateBookingAsync(UpdateBookingRequest dto)
        {
            var existingEvent = await _bookingRepositorie.GetByIdAsync(dto.id);
            dto.Adapt(existingEvent);
            await _bookingRepositorie.SaveChangesAsync();
            return dto;
        }
    }
}
