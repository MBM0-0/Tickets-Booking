using System.Net;
using System.Text.Json;
using TicketsBooking.Application.Exceptions;

namespace TicketsBooking.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message = exception.Message;

            switch (exception)
            {
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;
                case ValidationException:
                    status = HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedException:
                    status = HttpStatusCode.Unauthorized;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "Internal Server Error";
                    break;
            }

            var response = new { StatusCode = (int)status, Message = message };
            var payload = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(payload);
        }
    }
}