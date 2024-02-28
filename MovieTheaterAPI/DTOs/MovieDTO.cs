using MovieTheaterAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieTheaterAPI.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string MovieName { get; set; } = null!;
        [DataType(DataType.Date)]
        public string? ReleaseDate { get; set; }
        [DataType(DataType.Date)]
        public string? EndDate { get; set; }
        public double? Rating { get; set; }
        public string Trailer { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Synopsis { get; set; } = null!;
        public int Length { get; set; }
        public string Poster { get; set; } = null!;
        public string Thumbnail { get; set; } = null!;
        public string Logo { get; set; } = null!;
        public int AgeRestricted { get; set; }
        public List<GenreDTO>? Genres { get; } = [];
        public List<DirectorDTO>? Directors { get; } = [];
        public List<CastDTO>? Casts { get; } = [];
        public ICollection<MovieGenreDTO>? MovieGenres { get; set; } = new List<MovieGenreDTO>();
        public ICollection<MovieDirectorDTO>? MovieDirectors { get; set; } = new List<MovieDirectorDTO>();
        public ICollection<MovieCastDTO>? MovieCasts { get; set; } = new List<MovieCastDTO>();
        public ICollection<ScheduleDTO>? Schedules { get;  } = new List<ScheduleDTO>();
    }
}
