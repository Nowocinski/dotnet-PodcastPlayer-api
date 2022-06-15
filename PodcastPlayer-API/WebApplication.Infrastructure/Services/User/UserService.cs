using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTOs;
using WebApplication.Infrastructure.Extensions;

namespace WebApplication.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Core.Models.User> _genericRepository;
        public UserService(IGenericRepository<Core.Models.User> genericRepository)
        {
            this._genericRepository = genericRepository;
        }
        public async Task<LoginDTO> LoginAsync(string Email, string Password)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(RegisterCommand data)
        {
            var users = await this._genericRepository.GetAll();
            if(users.FirstOrDefault(u => u.Email == data.Email) != null)
            {
                throw new Exception($"User e-mail: '{data.Email}' already exists.");
            }
            if (users.FirstOrDefault(u => u.Username == data.Username) != null)
            {
                throw new Exception($"Username: '{data.Username}' already exists.");
            }

            var user = new Core.Models.User
            {
                Id = Guid.NewGuid(),
                Email = data.Email,
                Password = data.Password.Hash(),
                FirstName = data.FirstName,
                LastName = data.LastName
            };
            await this._genericRepository.Insert(user);
            await this._genericRepository.Save();
        }

        public async Task<IEnumerable<AccountDTO>> GetAll()
        {
            var users = await this._genericRepository.GetAll();
            return users.Select(u => new AccountDTO
            {
                Id = Guid.NewGuid(),
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToList();
        }
    }
}
