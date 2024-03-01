using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> GetAllMovies();
        Task<MovieDTO> GetMovieById(int id);
        Task<MovieDTO> CreateMovie(MovieDTO movie);
        Task UpdateMovie(int id, MovieDTO movie);
        Task DeleteMovie(int id);
        Task<IEnumerable<MovieDTO>> GetMoviesByGenre(int genreId);
        Task<MovieDTO> GetMovieDetails(int id);
        Task<IEnumerable<MovieDTO>> GetMoviesShowing();
        Task<IEnumerable<MovieDTO>> GetTopMovies();
        Task<IEnumerable<MovieDTO>> GetMoviesEnd();
        Task<IEnumerable<MovieDTO>> GetMoviesUpcoming();
        Task<IEnumerable<MovieDTO>> GetMovieByDirector(int directorId);

    }

}
