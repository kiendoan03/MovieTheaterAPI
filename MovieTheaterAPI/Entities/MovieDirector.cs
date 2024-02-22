namespace MovieTheaterAPI.Entities
{
    public class MovieDirector
    {
        public int? MovieId { get; set; }
        public int? DirectorId { get; set; }
        public Movie? Movies { get; set; } = null;
        public Director? Directors { get; set; } = null;
    }
}
