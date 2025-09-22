
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TicketsBooking.Application;
using TicketsBooking.Application.BackgroundJobs;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Application.Services;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Repositories;
namespace TicketsBooking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationDbContext>(Options => Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddScoped<IEventRepositorie, EventRepositorie>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IUserRepositorie, UserRepositorie>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBookingRepositorie, BookingRepositorie>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            builder.Services.AddHostedService<EventExpirationService>();

            var app = builder.Build();

            app.UseMiddleware<TicketsBooking.Application.Middleware.ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
