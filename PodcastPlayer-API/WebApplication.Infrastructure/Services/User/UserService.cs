using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.Commands;
using WebApplication.Infrastructure.DTOs;
using WebApplication.Infrastructure.Extensions;
using WebApplication.Infrastructure.Services.User.JwtToken;

namespace WebApplication.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Core.Models.User> _genericRepository;
        private readonly IJwtHandler _jwtHandler;
        public UserService(IGenericRepository<Core.Models.User> genericRepository, IJwtHandler jwtHandler)
        {
            this._genericRepository = genericRepository;
            this._jwtHandler = jwtHandler;
        }
        public async Task<LoginDTO> LoginAsync(string Email, string Password)
        {
            var users = await _genericRepository.GetAll();
            Core.Models.User user = users.FirstOrDefault(u => u.Email == Email);
            if (user == null)
                throw new Exception("Invalid credentials.");
            if (user.Password != Password.Hash())
                throw new Exception("Invalid credentials.");

            // TODO: User does not have any roles
            string token = _jwtHandler.CreateToken(user.Id, String.Empty);

            return new LoginDTO
            {
                Token = token,
                Id = Guid.NewGuid(),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
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
                LastName = data.LastName,
                Username = data.Username
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
