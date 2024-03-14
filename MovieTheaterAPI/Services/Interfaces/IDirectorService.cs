using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IDirectorService
    {
        Task<IEnumerable<DirectorDTO>> GetAllDirectors();
        Task<DirectorDTO> GetDirectorById(int id);
        Task<DirectorDTO> CreateDirector(DirectorDTO director, IFormFile file);
        Task UpdateDirector( DirectorDTO director,int id, IFormFile? file);
        Task DeleteDirector(int id);
    }

}
