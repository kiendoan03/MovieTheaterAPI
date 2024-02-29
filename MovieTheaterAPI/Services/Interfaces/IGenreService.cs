using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDTO>> GetAllGenres();
        Task<GenreDTO> GetGenreById(int id);
        Task<GenreDTO> CreateGenre(GenreDTO genre);
        Task UpdateGenre(int id, GenreDTO genre);
        Task DeleteGenre(int id);
    }

}
