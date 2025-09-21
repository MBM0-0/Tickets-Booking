using Mapster;
using TicketsBooking.Application.DTOs.User;
using TicketsBooking.Application.Exceptions;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;


namespace TicketsBooking.Application.Services
{
    public class UserService :IUserService
    {
        private readonly IUserRepositorie _userRepositorie;
    public UserService(IUserRepositorie userRepositorie)
    {
            _userRepositorie = userRepositorie;
    }
    
    public async Task<List<UserResponse>> GetAllUserAsync()
        {
            var events = await _userRepositorie.GetAllAsync();
            if (events == null || !events.Any())
                throw new NotFoundException("There is No Users to Show.");

            return events.Adapt<List<UserResponse>>();
        }
    public async Task <UserResponse> GetByUserIdAsync(int id)
    {
      var entity = await _userRepositorie.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("There is No User Found With This Id.");
            return entity.Adapt<UserResponse>();
    }
    public async Task<UserResponse> AddUserAsync(CreateUserRequest dto)
        {
            var entity = dto.Adapt<User>();
            if ( entity.Password.Length is < 8 || !(entity.Password.Any(char.IsDigit)) || !(entity.Password.Any(char.IsUpper)) || !(entity.Password.Any(char.IsLower)))
            throw new ValidationException("Password must be more than 8 characters and contain a number, uppercase and lowercase letters.");
            if (!entity.Email.Contains('@') || !(entity.Email.EndsWith(".com")))
                throw new ValidationException("Email Not Valid.");
            var emailexisit = await _userRepositorie.GetByEmailAsync(entity.Email);
            if (emailexisit != null)
                throw new ValidationException("The Email You Entered is Used Try Another One.");
            var userexisit = await _userRepositorie.GetByUsernameAsync(entity.username);
            if (userexisit != null)
                throw new ValidationException("The username You Entered is Used Try Another One.");

            await _userRepositorie.AddAsync(entity);
            await _userRepositorie.SaveChangesAsync();
            return entity.Adapt<UserResponse>();
        }
    public async Task<UpdateUserRequest> UpdateUserAsync(UpdateUserRequest dto)
        {
            var entity = await _userRepositorie.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new NotFoundException("There is No User Found With This Id.");
            if (dto.Password.Length is < 8 || !(dto.Password.Any(char.IsDigit)) || !(dto.Password.Any(char.IsUpper)) || !(dto.Password.Any(char.IsLower)))
                throw new ValidationException("Password must be more than 8 characters and contain a number, uppercase and lowercase letters.");
            if (!dto.Email.Contains('@') || !(dto.Email.EndsWith(".com")))
                throw new ValidationException("Email Not Valid.");
            var emailexisit = await _userRepositorie.GetByEmailAsync(dto.Email);
            if (emailexisit != null)
                throw new ValidationException("The Email You Entered is Used Try Another One.");

            dto.Adapt(entity);
            await _userRepositorie.SaveChangesAsync();
            return dto;
        }
    public async Task DeleteUserAsync(int id)
        {
            var entity = await _userRepositorie.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("There is No User Found With This Id.");
            await _userRepositorie.DeleteAsync(entity);
           await _userRepositorie.SaveChangesAsync(); 
        }
    }
}
