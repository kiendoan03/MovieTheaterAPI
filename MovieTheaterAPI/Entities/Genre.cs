namespace MovieTheaterAPI.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string GenreName { get; set; } = null!;
        public List<Movie> Movies { get; } = [];
        public ICollection<MovieGenre>? MovieGenres { get;  } = new List<MovieGenre>();

    }
}
