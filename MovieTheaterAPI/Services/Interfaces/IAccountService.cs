using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.DTOs;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserDTO> Login([FromBody] LoginDTO loginDTO);
    }
}
