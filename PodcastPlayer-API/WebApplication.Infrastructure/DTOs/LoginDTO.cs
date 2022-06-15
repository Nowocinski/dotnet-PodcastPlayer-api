namespace WebApplication.Infrastructure.DTOs
{
    public class LoginDTO
    {
        public string Token { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
