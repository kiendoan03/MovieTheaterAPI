using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;

namespace MovieTheaterAPI.Repository
{
    public class DirectorRepository : Repository<Director>, IDirectorRepository
    {
        public DirectorRepository(MovieTheaterDbContext context) : base(context)
        {
        }
    }
}
