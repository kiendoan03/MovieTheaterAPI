using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface ICastRepository : IRepository<Cast>
    {
        Task<IEnumerable<Cast>> GetMovieByCast(int castId);
    }
}
