namespace WebApplication.Infrastructure.Services.User.JwtToken
{
    public interface IJwtHandler
    {
        string CreateToken(Guid UserId, string role);
    }
}
