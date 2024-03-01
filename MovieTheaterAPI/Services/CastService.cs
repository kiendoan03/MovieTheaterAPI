using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Services
{
    public class CastService : ICastService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CastService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CastDTO>> GetAll()
        {
            var casts = await _unitOfWork.CastRepository.GetAll();
            return _mapper.Map<IEnumerable<CastDTO>>(casts);
        }

        public async Task<CastDTO> GetById(int id)
        {
            return _mapper.Map<CastDTO>(await _unitOfWork.CastRepository.GetById(id));
        }

        public async Task<CastDTO> CreateCast(CastDTO castDTO)
        {
            var newCast = _mapper.Map<Cast>(castDTO);
            await _unitOfWork.CastRepository.Add(newCast);
            await _unitOfWork.Save();

            return _mapper.Map<CastDTO>(newCast);
        }

        public async Task Update(CastDTO entity)
        {
            var cast = await _unitOfWork.CastRepository.GetById(entity.Id);
            if (cast == null)
            {
                // Handle not found
                return;
            }

            _mapper.Map(entity, cast);
            await _unitOfWork.Save();
        }

        public async Task Delete(int id)
        {
            var cast = await _unitOfWork.CastRepository.GetById(id);
            await _unitOfWork.CastRepository.Delete(cast);
            await _unitOfWork.Save();
        }

        public async Task<bool> IsExists(int id)
        {
            return await _unitOfWork.CastRepository.IsExists(id);
        }

        public async Task<IEnumerable<CastDTO>> GetMovieByCast(int castId)
        {
            var movies = await _unitOfWork.CastRepository.GetMovieByCast(castId);
            return _mapper.Map<IEnumerable<CastDTO>>(movies);
        }
    }



}
