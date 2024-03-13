using MovieTheaterAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieTheaterAPI.DTOs
{
    public class MovieDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string MovieName { get; set; } = null!;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string? ReleaseDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string? EndDate { get; set; }
        [Range(0, 10)]
        public double? Rating { get; set; }
        public string? Trailer { get; set; }
        public string Language { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Synopsis { get; set; } = null!;
        public int Length { get; set; }
        public string? Poster { get; set; } 
        public string? Thumbnail { get; set; } 
        public string? Logo { get; set; }
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
