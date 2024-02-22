using MovieTheaterAPI.Entities;
namespace MovieTheaterAPI.Repository
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<Movie> GetMovieWithFk(int id);
    }
}
