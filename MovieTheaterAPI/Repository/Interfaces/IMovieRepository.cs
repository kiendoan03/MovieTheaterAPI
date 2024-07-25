using MovieTheaterAPI.Entities;
namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<Movie> GetMovieWithFk(int id);
        Task<Movie> GetMovieDetails(int id);
        Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId);
        Task<IEnumerable<Movie>> GetMoviesShowing();
        Task<IEnumerable<Movie>> GetTopMovies();
        Task<IEnumerable<Movie>> GetMoviesEnd();
        Task<IEnumerable<Movie>> GetMoviesUpcoming();
        Task<IEnumerable<Movie>> GetMovieByDirector(int directorId);
        //count movies showing
        Task<int> CountMoviesShowing();
        Task<int> CountMoviesUpcomming();
        Task<int> CountMoviesEnd();
        //GetMoviesByKeywork
        Task<IEnumerable<Movie>> GetMoviesByKeywork(string keyword);
        //CheckMovieExistInTicket
        Task<bool> CheckMovieExistInTicket(int movieId);
    }
}
