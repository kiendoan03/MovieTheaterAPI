using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class DirectorDTO
    {
        public int Id { get; set; }
        public string DirectorName { get; set; } = null!;
        public string? directorImage { get; set; } = null!;
        public List<MovieDTO> Movies { get;  } = [];
        public ICollection<MovieDirectorDTO>? MovieDirectors { get;  } = new List<MovieDirectorDTO>();

    }
}
