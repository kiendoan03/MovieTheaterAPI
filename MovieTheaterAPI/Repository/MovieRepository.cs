using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace MovieTheaterAPI.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieTheaterDbContext context) : base(context)
        {
        }

        public async Task<Movie> GetMovieWithFk(int id)
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                .Include(m => m.MovieDirectors)
                .Include(m => m.MovieCasts)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
