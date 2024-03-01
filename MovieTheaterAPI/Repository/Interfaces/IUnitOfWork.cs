namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository MovieRepository { get; }
        IGenreRepository GenreRepository { get; }
        IDirectorRepository DirectorRepository { get; }
        IRoomRepository RoomRepository { get; }
        ICastRepository CastRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IScheduleRepository ScheduleRepository { get; }
        ITicketRepository TicketRepository { get; }
        IStaffRepository StaffRepository { get; }
        ISeatRepository SeatRepository { get; }
        Task Save();
    }
}
