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
        public async Task<IActionResult> Show()
        {
            var result = await _bookingService.GetAllBookingAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ShowById(int id)
        {
            var result = await _bookingService.GetBookingById(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Creat(CreateBookingRequest dto)
        {
            var result = await _bookingService.AddBookingAsync(dto);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> update (UpdateBookingRequest dto)
        {
        var result = await _bookingService.UpdateBookingAsync(dto);
        return Ok(result);
        }
    }
}
