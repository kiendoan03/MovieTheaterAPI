using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;

namespace MovieTheaterAPI.Repository
{
    public class GenreRepository : Repository<Genre> , IGenreRepository
    {
        public GenreRepository(MovieTheaterDbContext context) : base(context)
        {
        }
    }
}
