using MovieTheaterAPI.DTOs;
using System.ComponentModel.DataAnnotations;

namespace MovieTheaterAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string MovieName { get; set; } = null!;
        [DataType(DataType.Date)]
        public DateOnly? ReleaseDate { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? EndDate { get; set; }
        public double? Rating { get; set; }
        public string Trailer { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Synopsis { get; set; } = null!;
        public string Poster { get; set; } = null!;
        public string Thumbnail { get; set; } = null!;
        public string Logo { get; set; } = null!;
        public int AgeRestricted { get; set; }
        public List<Genre>? Genres { get;  } = [];
        public List<Director>? Directors { get;  } = [];
        public List<Cast>? Casts { get;  } = [];
        public ICollection<MovieGenre>? MovieGenres { get; set; } = new List<MovieGenre>();
        public ICollection<MovieDirector>? MovieDirectors { get; set; } = new List<MovieDirector>();
        public ICollection<MovieCast>? MovieCasts { get; set; } = new List<MovieCast>();
        public ICollection<Schedule>? Schedules { get;  } = new List<Schedule>();
    }
}
