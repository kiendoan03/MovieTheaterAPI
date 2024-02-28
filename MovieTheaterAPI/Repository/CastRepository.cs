using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public class CastRepository : Repository<Cast>, ICastRepository
    {
        public CastRepository(MovieTheaterDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Cast>> GetMovieByCast(int castId)
        {
            return await _context.Casts.Where(x => x.Id == castId)
                .Include(x => x.Movies)
                .ToListAsync();
        }
    }       
}
