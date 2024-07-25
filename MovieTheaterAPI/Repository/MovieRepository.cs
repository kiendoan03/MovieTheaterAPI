using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MovieTheaterAPI.Repository.Interfaces;

namespace MovieTheaterAPI.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieTheaterDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckMovieExistInTicket(int movieId)
        {
            var tickets = from t in _context.Tickets
                          where t.Schedules.MovieId == movieId
                          select t;
            return await Task.FromResult(tickets.Any());
        }

        public async Task<int> CountMoviesEnd()
        {
            var datetime = DateTime.Now;
            var dateonly = new DateOnly(datetime.Year, datetime.Month, datetime.Day);
            var movies = from m in _context.Movies
                         where m.EndDate < dateonly
                         select m;
            return await Task.FromResult(movies.Count());
        }

        public async Task<int> CountMoviesShowing()
        {
            var datetime = DateTime.Now;
            var dateonly = new DateOnly(datetime.Year, datetime.Month, datetime.Day);
            var movies = from m in _context.Movies
                         where m.ReleaseDate <= dateonly
                         where m.EndDate >= dateonly
                         select m;
            return await Task.FromResult(movies.Count());
        }

        public async Task<int> CountMoviesUpcomming()
        {
            var datetime = DateTime.Now;
            var dateonly = new DateOnly(datetime.Year, datetime.Month, datetime.Day);
            var movies = from m in _context.Movies
                         where m.ReleaseDate > dateonly
                         select m;
            return await Task.FromResult(movies.Count());
        }

        public async Task<IEnumerable<Movie>> GetMovieByDirector(int directorId)
        {
            return await _context.Movies
                .Where(m => m.Directors.Any(d => d.Id == directorId))
                .ToListAsync();
        }

        public async Task<Movie> GetMovieDetails(int id)
        {
            return await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Directors)
                .Include(m => m.Casts)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId)
        {
            return await _context.Movies
                .Include(m => m.Genres)
                .Where(m => m.Genres.Any(g => g.Id == genreId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByKeywork(string keyword)
        {
            var movies = from m in _context.Movies
                         where m.MovieName.Contains(keyword)
                         select m;
            return await Task.FromResult(movies);
        }

        public async Task<IEnumerable<Movie>> GetMoviesEnd()
        {
            var datetime = DateTime.Now;
            var dateonly = new DateOnly(datetime.Year, datetime.Month, datetime.Day);
            var movies = from m in _context.Movies
                         where m.EndDate < dateonly
                         select m;
            return await Task.FromResult(movies);
        }

        public async Task<IEnumerable<Movie>> GetMoviesShowing()
        {
            var datetime = DateTime.Now;
            var dateonly = new DateOnly(datetime.Year, datetime.Month, datetime.Day);

            var movies = from m in _context.Movies
                         where m.ReleaseDate <= dateonly 
                         where m.EndDate >= dateonly
                         select m;
            return await Task.FromResult(movies);
        }

        public async Task<IEnumerable<Movie>> GetMoviesUpcoming()
        {
            var datetime = DateTime.Now;
            var dateonly = new DateOnly(datetime.Year, datetime.Month, datetime.Day);
            var movies = from m in _context.Movies
                         where m.ReleaseDate > dateonly
                         select m;
            return await Task.FromResult(movies);
        }

        public async Task<Movie> GetMovieWithFk(int id)
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                .Include(m => m.MovieDirectors)
                .Include(m => m.MovieCasts)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Movie>> GetTopMovies()
        {
            return await _context.Movies.OrderByDescending(m => m.Rating).Take(4).ToListAsync();
        }
    }
}
