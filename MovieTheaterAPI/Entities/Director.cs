namespace MovieTheaterAPI.Entities
{
    public class Director
    {
        public int Id { get; set; }
        public string DirectorName { get; set; } = null!;
        public string directorImage { get; set; } = null!;
        public List<Movie> Movies { get;  } = [];
        public ICollection<MovieDirector>? MovieDirectors { get;  } = new List<MovieDirector>();
    }
}
