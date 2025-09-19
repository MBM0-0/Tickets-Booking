using TicketsBooking.Application.DTOs.Booking;

namespace TicketsBooking.Application.Interfaces
{
    public interface IBookingService
    {
        public Task<UpdateBookingRequest> UpdateBookingAsync(UpdateBookingRequest dto);
        public Task<CreateBookingRequest> AddBookingAsync(CreateBookingRequest dto);
        public Task<BookingResponse> GetBookingById(int id);
        public Task<List<BookingResponse>> GetAllBookingAsync();

    }
}
