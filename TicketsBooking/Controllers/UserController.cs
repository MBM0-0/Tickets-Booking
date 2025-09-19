using Microsoft.AspNetCore.Mvc;
using TicketsBooking.Application.DTOs.User;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Application.Services;

namespace TicketsBooking.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
    private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
     public async Task<IActionResult> Users ()
        {
            var Users = await _userService.GetAllUserAsync();
            return Ok(Users);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> User (int id)
        {
            var user = await _userService.GetByUserId(id);
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> Add (CreateUserRequest dto)
        {
            var entity = await _userService.AddUserAsync(dto);
            return Ok(entity);
        }
        [HttpPut]
        public async Task<IActionResult> Update (UpdateUserRequest dto)
        {
            var entity = await _userService.UpdateUserAsync(dto);
            return Ok(entity);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete (int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }

    }
}
