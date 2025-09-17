using Microsoft.AspNetCore.Mvc;
using TicketsBooking.Application.DTOs;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Application.Services;

namespace TicketsBooking.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpGet]
        //[Route]
        public async Task<IActionResult> Events ()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }
        [HttpPost]
        public async Task<IActionResult> Create (CreateEventRequest dto)
        {
            var events = await _eventService.CreateEventAsync(dto);
            return Ok(events);
        }
    }
}
