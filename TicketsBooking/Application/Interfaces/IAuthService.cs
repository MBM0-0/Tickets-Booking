using TicketsBooking.Application.DTOs.Auth;

namespace TicketsBooking.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request);
    }
}
