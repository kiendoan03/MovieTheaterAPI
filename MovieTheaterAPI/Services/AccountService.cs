using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountService(ITokenService tokenService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<UserDTO> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());
            if (user == null) throw new UnauthorizedAccessException("Invalid email");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded) throw new UnauthorizedAccessException("Invalid password");
             
            return new UserDTO
            {
                Username = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user),
            };
        }
    }
}
