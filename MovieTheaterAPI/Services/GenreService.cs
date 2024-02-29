using AutoMapper;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GenreDTO>> GetAllGenres()
        {
            var genres = await _unitOfWork.GenreRepository.GetAll();
            return _mapper.Map<IEnumerable<GenreDTO>>(genres);
        }

        public async Task<GenreDTO> GetGenreById(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetById(id);
            return _mapper.Map<GenreDTO>(genre);
        }

        public async Task<GenreDTO> CreateGenre(GenreDTO genre)
        {
            var newGenre = _mapper.Map<Genre>(genre);
            await _unitOfWork.GenreRepository.Add(newGenre);
            await _unitOfWork.Save();
            return _mapper.Map<GenreDTO>(newGenre);
        }

        public async Task UpdateGenre(int id, GenreDTO genre)
        {
            if (id != genre.Id)
            {
                throw new ArgumentException("Id mismatch");
            }

            var updatedGenre = _mapper.Map<Genre>(genre);
            await _unitOfWork.GenreRepository.Update(updatedGenre);
            await _unitOfWork.Save();
        }

        public async Task DeleteGenre(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetById(id);
            await _unitOfWork.GenreRepository.Delete(genre);
            await _unitOfWork.Save();
        }
    }

}
