using Microsoft.AspNetCore.Mvc;
using TicketsBooking.Application.DTOs.Event;
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
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var events = await _eventService.GetEventByIdAsync(id);
            return Ok(events);

        }
        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventRequest dto)
        {
            var events = await _eventService.CreateEventAsync(dto);
            return Ok(events);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateEvent(UpdateEventRequest dto)
        {
            var events = await _eventService.UpdateEventAsync(dto);
            return Ok(events);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _eventService.DeleteEventAsync(id);
            return Ok();
        }
    }
}
