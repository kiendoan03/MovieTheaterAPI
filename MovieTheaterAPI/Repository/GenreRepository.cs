using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public class GenreRepository : Repository<Genre> , IGenreRepository
    {
        public GenreRepository(MovieTheaterDbContext context) : base(context)
        {
        }
    }
}
