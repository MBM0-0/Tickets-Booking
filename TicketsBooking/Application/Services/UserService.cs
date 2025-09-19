using Mapster;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.CodeDom.Compiler;
using TicketsBooking.Application.DTOs.Event;
using TicketsBooking.Application.DTOs.User;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Repositories;

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
            var dto = await _userRepositorie.GetAllAsync();
            return dto.Adapt<List<UserResponse>>();
        }
    public async Task <UserResponse> GetByUserId (int id)
    {
      var entity = await _userRepositorie.GetByIdAsync(id);
      return entity.Adapt<UserResponse>();
    }
    public async Task<UserResponse> AddUserAsync(CreateUserRequest dto)
        {
            var entity = dto.Adapt<User>();
            await _userRepositorie.AddAsync(entity);
            await _userRepositorie.SaveChangesAsync();
            return entity.Adapt<UserResponse>();
        }
    public async Task<UpdateUserRequest> UpdateUserAsync(UpdateUserRequest dto)
        {
            var entity = await _userRepositorie.GetByIdAsync(dto.Id);
            dto.Adapt(entity);
            await _userRepositorie.SaveChangesAsync();
            return dto;
        }
    public async Task DeleteUserAsync(int id)
        {
           await _userRepositorie.DeleteAsync(id);
           await _userRepositorie.SaveChangesAsync(); 
        }
    }
}
