using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);

    }
}
