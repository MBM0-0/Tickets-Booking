using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.Application.DTOs.Booking;
using TicketsBooking.Application.Exceptions;
using TicketsBooking.Application.Interfaces;

namespace TicketsBooking.Controllers
{
    [Authorize]
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
            try
            {
                var result = await _bookingService.GetAllBookingAsync();
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var result = await _bookingService.GetBookingByIdAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("SeatAvailability/{id}")]
        public async Task<IActionResult> GetSeatAvailability(int id)
        {
            try
            {
                var result = await _bookingService.GetSeatAvailabilityAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingRequest dto)
        {
            try
            {
                var result = await _bookingService.AddBookingAsync(dto);
                return CreatedAtAction(nameof(CreateBooking), new { id = result.Id }, result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBooking(UpdateBookingRequest dto)
        {
            try
            {
                var result = await _bookingService.UpdateBookingAsync(dto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message }); 
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPatch("Cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                await _bookingService.CancelBookingAsync(id);
                return Ok(new { Message = $"The Booking With Id {id} Has Been Cancelled" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
