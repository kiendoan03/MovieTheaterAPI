using AutoMapper;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MovieDTO> CreateMovie(MovieDTO movie, MovieFiles files)
        {
            var newMovie = _mapper.Map<Movie>(movie);

            var poster = files.poster;
            var thumbnail = files.thumbnail;
            var logo = files.logo;
            var trailer = files.trailer;
            if (poster.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "movies", "posters");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + newMovie.MovieName + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);

                if (poster.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        poster.CopyTo(stream);
                    }
                }

                newMovie.Poster = "/uploads/images/movies/posters/" + fileName;
            }
            if(thumbnail.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "movies", "thumbnails");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + newMovie.MovieName + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);

                if (thumbnail.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        thumbnail.CopyTo(stream);
                    }
                }

                newMovie.Thumbnail = "/uploads/images/movies/thumbnails/" + fileName;
            }
            if(logo.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "movies", "logos");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + newMovie.MovieName + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);

                if (logo.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        logo.CopyTo(stream);
                    }
                }

                newMovie.Logo = "/uploads/images/movies/logos/" + fileName;
            }
            if(trailer.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "movieTrailers");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + newMovie.MovieName + ".mp4";
                var fullPath = Path.Combine(pathToSave, fileName);

                if (trailer.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        trailer.CopyTo(stream);
                    }
                }

                newMovie.Trailer = "/uploads/videos/movieTrailers/" + fileName;
            }

            await _unitOfWork.MovieRepository.Add(newMovie);
            await _unitOfWork.Save();
            return _mapper.Map<MovieDTO>(newMovie);
        }

        public async Task DeleteMovie(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);
            await _unitOfWork.MovieRepository.Delete(movie);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<MovieDTO>> GetAllMovies()
        {
            var movies = await _unitOfWork.MovieRepository.GetAll();
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<IEnumerable<MovieDTO>> GetMovieByDirector(int directorId)
        {
            var movies = await _unitOfWork.MovieRepository.GetMovieByDirector(directorId);
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<MovieDTO> GetMovieById(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);
            return _mapper.Map<MovieDTO>(movie);
        }

        public async Task<MovieDTO> GetMovieDetails(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetMovieDetails(id);
            return _mapper.Map<MovieDTO>(movie);
        }

        public async Task<IEnumerable<MovieDTO>> GetMoviesByGenre(int genreId)
        {
            var movies = await _unitOfWork.MovieRepository.GetMoviesByGenre(genreId);
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<IEnumerable<MovieDTO>> GetMoviesEnd()
        {
            var movies = await _unitOfWork.MovieRepository.GetMoviesEnd();
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<IEnumerable<MovieDTO>> GetMoviesShowing()
        {
            var movies = await _unitOfWork.MovieRepository.GetMoviesShowing();
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<IEnumerable<MovieDTO>> GetMoviesUpcoming()
        {
            var movies = await _unitOfWork.MovieRepository.GetMoviesUpcoming();
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<IEnumerable<MovieDTO>> GetTopMovies()
        {
            var movies = await _unitOfWork.MovieRepository.GetTopMovies();
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task UpdateMovie(int id, MovieDTO movie)
        {
            if (id != movie.Id)
            {
                throw new ArgumentException("Id is not valid");
            }
            var movieExists = await _unitOfWork.MovieRepository.GetMovieWithFk(id);
            _mapper.Map(movie, movieExists);
            await _unitOfWork.MovieRepository.Update(movieExists);
            await _unitOfWork.Save();
        }
    }
}
