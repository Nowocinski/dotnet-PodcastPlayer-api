using Microsoft.AspNetCore.Mvc;
using WebApplication.Infrastructure.DTOs;
using WebApplication.Infrastructure.Services.User;

namespace PodcastPlayer_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<AccountDTO>> Get()
        {
            return await this._userService.GetAll();
        }
    }
}
