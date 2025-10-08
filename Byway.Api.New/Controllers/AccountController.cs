using Byway.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Byway.Application.DTOs.Auth;

namespace Byway.Api.New.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            var userDto = await _accountService.RegisterAsync(registerDto);
            return Ok(userDto);
        }




        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var userDto = await _accountService.LoginAsync(loginDto);
            return Ok(userDto);
        }

    }
}
