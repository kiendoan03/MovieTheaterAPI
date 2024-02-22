
using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;

namespace MovieTheaterAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieTheaterDbContext _context;
        public UnitOfWork(MovieTheaterDbContext context)
        {
            _context = context;
            MovieRepository = new MovieRepository(_context);
            GenreRepository = new GenreRepository(_context);
            DirectorRepository = new DirectorRepository(_context);
            RoomRepository = new RoomRepository(_context);
            CastRepository = new CastRepository(_context);
            CustomerRepository = new CustomerRepository(_context);
            ScheduleRepository = new ScheduleRepository(_context);
            TicketRepository = new TicketRepository(_context);
            StaffRepository = new StaffRepository(_context);
            SeatRepository = new SeatRepository(_context);
        }
        public IMovieRepository MovieRepository { get; private set; }

        public IGenreRepository GenreRepository { get; private set; }

        public IDirectorRepository DirectorRepository { get; private set; }

        public IRoomRepository RoomRepository { get; private set; }

        public ICastRepository CastRepository { get; private set; }

        public ICustomerRepository CustomerRepository { get; private set; }

        public IScheduleRepository ScheduleRepository { get; private set; }

        public ITicketRepository TicketRepository { get; private set; }

        public IStaffRepository StaffRepository { get; private set; }
        public ISeatRepository SeatRepository { get; private set; }


        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
