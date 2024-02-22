using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string GenreName { get; set; } = null!;
        public List<MovieDTO> Movies { get;  } = [];
        public ICollection<MovieGenreDTO>? MovieGenres { get;  } = new List<MovieGenreDTO>();

    }
}
