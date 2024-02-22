using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class CastDTO
    {
        public int Id { get; set; }
        public string CastName { get; set; } = null!;
        public string CastImage { get; set; } = null!;
        public List<MovieDTO> Movies { get;  } = [];
        public ICollection<MovieCastDTO>? MovieCasts { get;  } = new List<MovieCastDTO>();

    }
}
