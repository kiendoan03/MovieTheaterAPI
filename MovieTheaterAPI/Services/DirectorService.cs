using AutoMapper;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DirectorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DirectorDTO>> GetAllDirectors()
        {
            var directors = await _unitOfWork.DirectorRepository.GetAll();
            return _mapper.Map<IEnumerable<DirectorDTO>>(directors);
        }

        public async Task<DirectorDTO> GetDirectorById(int id)
        {
            var director = await _unitOfWork.DirectorRepository.GetById(id);
            return _mapper.Map<DirectorDTO>(director);
        }

        public async Task<DirectorDTO> CreateDirector(DirectorDTO director)
        {
            var newDirector = _mapper.Map<Director>(director);
            await _unitOfWork.DirectorRepository.Add(newDirector);
            await _unitOfWork.Save();
            return _mapper.Map<DirectorDTO>(newDirector);
        }

        public async Task UpdateDirector(int id, DirectorDTO director)
        {
            if (id != director.Id)
            {
                throw new ArgumentException("Id mismatch");
            }

            var updatedDirector = _mapper.Map<Director>(director);
            await _unitOfWork.DirectorRepository.Update(updatedDirector);
            await _unitOfWork.Save();
        }

        public async Task DeleteDirector(int id)
        {
            var director = await _unitOfWork.DirectorRepository.GetById(id);
            await _unitOfWork.DirectorRepository.Delete(director);
            await _unitOfWork.Save();
        }
    }

}
