using TicketsBooking.Application.DTOs.User;

namespace TicketsBooking.Application.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserResponse>> GetAllUserAsync();
        public Task<UserResponse> GetByUserIdAsync(int id);
        public Task<UserResponse> AddUserAsync(CreateUserRequest dto);
        public Task<UpdateUserRequest> UpdateUserAsync(UpdateUserRequest dto);
        public Task DeleteUserAsync(int id);
    }
}
