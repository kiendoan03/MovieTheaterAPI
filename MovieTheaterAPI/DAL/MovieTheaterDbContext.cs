using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.DTOs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MovieTheaterAPI.DAL

{
    public class MovieTheaterDbContext : IdentityDbContext<User , IdentityRole<int>, int>
    {
        public MovieTheaterDbContext(DbContextOptions<MovieTheaterDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Director> Directors { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Cast> Casts { get; set; } = null!;
        public DbSet<MovieCast> MovieCasts { get; set; } = null!;
        public DbSet<MovieDirector> MovieDirectors { get; set; } = null!;
        public DbSet<MovieGenre> MovieGenres { get; set; } = null!; 
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<SeatType> SeatTypes { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Staff> Staffs { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<RoomType> RoomTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity<MovieGenre>(
                    j => j.HasOne<Genre>(mg => mg.Genre).WithMany(g => g.MovieGenres),
                    j => j.HasOne<Movie>(mg => mg.Movie).WithMany(m => m.MovieGenres)
                );
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Directors)
                .WithMany(d => d.Movies)
                .UsingEntity<MovieDirector>(
                    j => j.HasOne<Director>(md => md.Directors).WithMany(d => d.MovieDirectors),
                    j => j.HasOne<Movie>(md => md.Movies).WithMany(m => m.MovieDirectors)
                );
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Casts)
                .WithMany(c => c.Movies)
                .UsingEntity<MovieCast>(
                    j => j.HasOne<Cast>(mc => mc.Cast).WithMany(c => c.MovieCasts),
                    j => j.HasOne<Movie>(mc => mc.Movie).WithMany(m => m.MovieCasts)
                );
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Schedules)
                .WithOne(s => s.Movie)
                .HasForeignKey(s => s.MovieId);
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Schedules)
                .WithOne(s => s.Room)
                .HasForeignKey(s => s.RoomId);
            modelBuilder.Entity<RoomType>()
                .HasMany(rt => rt.Rooms)
                .WithOne(r => r.RoomType)
                .HasForeignKey(r => r.RoomTypeId);
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Seats)
                .WithOne(s => s.Room)
                .HasForeignKey(s => s.RoomId);
            modelBuilder.Entity<SeatType>()
                .HasMany(st => st.Seats)
                .WithOne(s => s.SeatType)
                .HasForeignKey(s => s.SeatTypeId);
            modelBuilder.Entity<Schedule>()
                 .HasMany(s => s.Seats)
                 .WithMany(se => se.Schedules)
                 .UsingEntity<Ticket>(
                     l => l.HasOne<Seat>(ss => ss.Seats).WithMany(e => e.Tickets).OnDelete(DeleteBehavior.Restrict),
                     r => r.HasOne<Schedule>(ss => ss.Schedules).WithMany(e => e.Tickets));
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Tickets)
                .WithOne(t => t.Customer)
                .HasForeignKey(t => t.CustomerId);
            modelBuilder.Entity<Staff>()
                .HasMany(s => s.Tickets)
                .WithOne(t => t.Staff)
                .HasForeignKey(t => t.StaffId);
            modelBuilder.Entity<IdentityRole<int>>().ToTable("AspNetRoles");
            List<IdentityRole<int>> roles = new List<IdentityRole<int>>
            {
                new IdentityRole<int>
                {
                    Id = 1,
                    Name = "Staff",
                    NormalizedName = "STAFF",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                 new IdentityRole<int>
                {
                    Id = 2,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole<int>
                {
                    Id = 3,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
            };
            modelBuilder.Entity<IdentityRole<int>>().HasData(roles);
        }
        //public DbSet<MovieTheaterAPI.DTOs.MovieDTO> MovieDTO { get; set; } = default!;
    }
}
