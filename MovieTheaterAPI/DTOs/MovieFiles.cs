namespace MovieTheaterAPI.DTOs
{
    public class MovieFiles
    {
        public IFormFile? poster { get; set; }
        public IFormFile? thumbnail { get; set; }
        public IFormFile? trailer { get; set; }
        public IFormFile? logo { get; set; }
    }
}
