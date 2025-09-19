namespace TicketsBooking.Application.DTOs.User
{
    public class CreateUserRequest
    {
        public string FullName { get; set; }
        public string username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
