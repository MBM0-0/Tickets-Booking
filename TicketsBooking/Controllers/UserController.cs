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
     public async Task<IActionResult> GetAllUsers()
        {
            var Users = await _userService.GetAllUserAsync();
            return Ok(Users);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserByIdUser(int id)
        {
            var user = await _userService.GetByUserIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser (CreateUserRequest dto)
        {
            var entity = await _userService.AddUserAsync(dto);
            return Created("",entity);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser (UpdateUserRequest dto)
        {
            var entity = await _userService.UpdateUserAsync(dto);
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

    }
}
