using Mapster;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.Application.DTOs.Booking;
using TicketsBooking.Application.Interfaces;

namespace TicketsBooking.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var result = await _bookingService.GetAllBookingAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("SeatAvailability/{id}")]
        public async Task<IActionResult> GetSeatAvailability(int id)
        {
            var result = await _bookingService.GetSeatAvailabilityAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingRequest dto)
        {
            var result = await _bookingService.AddBookingAsync(dto);
            return Created(result.Id.ToString(),result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBooking(UpdateBookingRequest dto)
        {
        var result = await _bookingService.UpdateBookingAsync(dto);
        return Ok(result);
        }

        [HttpPatch("Cancel")]
        public async Task<ActionResult> CancelBooking (int id)
        {
           await _bookingService.CancelBookingAsync(id);
            return Ok($"The Booking With The Id: {id} Has Been Cancelled");
        }
    }
}
