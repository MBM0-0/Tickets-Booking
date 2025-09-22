using TicketsBooking.Application.DTOs.Booking;

namespace TicketsBooking.Application.Interfaces
{
    public interface IBookingService
    {
        public Task<List<BookingResponse>> GetAllBookingAsync();
        public Task<BookingResponse> GetBookingByIdAsync(int id);
        public Task<BookingSeatAvailability> GetSeatAvailabilityAsync(int id);
        public Task<BookingResponse> AddBookingAsync(CreateBookingRequest dto);
        public Task<UpdateBookingRequest> UpdateBookingAsync(UpdateBookingRequest dto);
        public Task CancelBookingAsync(int id);
    }
}
