using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public interface ICastRepository : IRepository<Cast>
    {
        Task<IEnumerable<Cast>> GetMovieByCast(int castId);
    }
}
