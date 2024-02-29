using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IDirectorService
    {
        Task<IEnumerable<DirectorDTO>> GetAllDirectors();
        Task<DirectorDTO> GetDirectorById(int id);
        Task<DirectorDTO> CreateDirector(DirectorDTO director);
        Task UpdateDirector(int id, DirectorDTO director);
        Task DeleteDirector(int id);
    }

}
