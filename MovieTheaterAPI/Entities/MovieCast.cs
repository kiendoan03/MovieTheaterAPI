namespace MovieTheaterAPI.Entities
{
    public class MovieCast
    {
        public int? MovieId { get; set; }
        public int? CastId { get; set; }
        public Movie? Movie { get; set; } = null!;
        public Cast? Cast { get; set; } = null!;
    }
}
