using Byway.Application.Interfaces;
using Byway.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static Byway.Application.DTOs.Auth;

namespace Byway.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        // --- 1. Add a private field for the validator ---
        private readonly IValidator<RegisterDto> _validator;

        // --- 2. Inject the validator in the constructor ---
        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService, IValidator<RegisterDto> validator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _validator = validator;
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) throw new UnauthorizedAccessException("Invalid email or password.");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) throw new UnauthorizedAccessException("Invalid email or password.");

            return new UserDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
           
            await _validator.ValidateAndThrowAsync(registerDto);

            // This business logic now only runs if the DTO is valid.
            bool emailExists = await _userManager.Users.AnyAsync(u => u.Email == registerDto.Email.ToLower());
            if (emailExists)
            {
                // This is a business rule validation, which is different from DTO format validation.
                throw new ValidationException("Validation failed.", new[] { new ValidationFailure("email", "Email address is already in use.") });
            }

            var user = new AppUser
            {
                Email = registerDto.Email.ToLower(),
                UserName = registerDto.Username.ToLower(),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                // These are errors from the Identity system (e.g., duplicate username)
                var validationFailures = result.Errors.Select(e => new ValidationFailure(e.Code, e.Description));
                throw new ValidationException("User creation failed.", validationFailures);
            }

            return new UserDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token =  await _tokenService.CreateToken(user)
            };
        }
    }
}

