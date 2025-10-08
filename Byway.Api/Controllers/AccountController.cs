using Byway.Application.DTOs;
using Byway.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Byway.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;


        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
           
            var userDto = await _authService.RegisterAsync(registerDto);

            
            return Ok(userDto);
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
         
            var userDto = await _authService.LoginAsync(loginDto);

           
            return Ok(userDto);
        }


    }
}
