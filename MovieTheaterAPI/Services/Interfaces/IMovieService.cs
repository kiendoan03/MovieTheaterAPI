using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> GetAllMovies();
        Task<MovieDTO> GetMovieById(int id);
        Task<MovieDTO> CreateMovie(MovieDTO movie, MovieFiles files);
        Task UpdateMovie( MovieDTO movie, int id, MovieFiles? files);
        Task DeleteMovie(int id);
        Task<MovieDTO> GetMovieWithFK(int id);
        Task<IEnumerable<MovieDTO>> GetMoviesByGenre(int genreId);
        Task<MovieDTO> GetMovieDetails(int id);
        Task<IEnumerable<MovieDTO>> GetMoviesShowing();
        Task<IEnumerable<MovieDTO>> GetTopMovies();
        Task<IEnumerable<MovieDTO>> GetMoviesEnd();
        Task<IEnumerable<MovieDTO>> GetMoviesUpcoming();
        Task<IEnumerable<MovieDTO>> GetMovieByDirector(int directorId);
        //count movies
        Task<int> CountMovies();
        //count movies showing
        Task<int> CountMoviesShowing();
        Task<int> CountMoviesUpcomming();
        Task<int> CountMoviesEnd();
        //GetMoviesByKeywork
        Task<IEnumerable<MovieDTO>> GetMoviesByKeywork(string keyword);
    }

}
