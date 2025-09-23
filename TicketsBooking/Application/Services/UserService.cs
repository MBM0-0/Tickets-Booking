using Mapster;
using System;
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
            string Username = entity.Username;
            if (Username.Length < 5 || Username.Length > 16 || !char.IsLetter(Username[0] ) || !Username.All(c => char.IsLetterOrDigit(c) || c == '_') || Username.Contains(' '))
                throw new ValidationException("Username Shoud Be betwen 4 to 17 later begans with a letter exaptonly _ from charcters cant have white space.");
            if ( entity.Password.Length is < 8 || !(entity.Password.Any(char.IsDigit)) || !(entity.Password.Any(char.IsUpper)) || !(entity.Password.Any(char.IsLower)))
            throw new ValidationException("Password must be more than 8 characters and contain a number, uppercase and lowercase letters.");
            if (!entity.Email.Contains('@') || !(entity.Email.ToLower().EndsWith(".com") || !char.IsLetter(entity.Email[0])))
                throw new ValidationException("Email Not Valid.");
            var emailexisit = await _userRepositorie.GetByEmailAsync(entity.Email);
            if (emailexisit != null)
                throw new ValidationException("The Email You Entered is Used Try Another One.");
            var userexisit = await _userRepositorie.GetByUsernameAsync(entity.Username);
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
            if (!dto.Email.Contains('@') || !(dto.Email.ToLower().EndsWith(".com") || !char.IsLetter(dto.Email[0])))
                throw new ValidationException("Email Not Valid.");
            var emailexisit = await _userRepositorie.GetByEmailAsync(dto.Email);
            if ( emailexisit != null && entity.Email == dto.Email)
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
