namespace MovieTheaterAPI.Entities
{
    public class Cast
    {
        public int Id { get; set; }
        public string CastName { get; set; } = null!;
        public string CastImage { get; set; } = null!;
        public List<Movie> Movies { get;  } = [];
        public ICollection<MovieCast>? MovieCasts { get;  } = new List<MovieCast>();
    }
}
