using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketsBooking.Application.DTOs.Auth;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure;

namespace TicketsBooking.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ApplicationDbContext _context;

        public AuthService(JwtOptions jwtOptions, ApplicationDbContext context)
        {
            _jwtOptions = jwtOptions;
            _context = context;
        }

        public async Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
                return null;

            var key = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                })
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new AuthenticationResponse
            {
                Token = jwt,
                ExpiresAt = tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTime)
            };
        }
    }
}