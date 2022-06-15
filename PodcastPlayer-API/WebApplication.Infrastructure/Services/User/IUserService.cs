using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTOs;

namespace WebApplication.Infrastructure.Services.User
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterCommand Data);
        Task<LoginDTO> LoginAsync(string Email, string Password);
        Task<IEnumerable<AccountDTO>> GetAll();
    }
}
