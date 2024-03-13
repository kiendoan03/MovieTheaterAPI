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
        private readonly IWebHostEnvironment _env;

        public CastService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
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

        public async Task<CastDTO> CreateCast(CastDTO castDTO, IFormFile file)
        {
            var newCast = _mapper.Map<Cast>(castDTO);
            //var files = castDTO.CastImage;
            var files = file;
            if (files.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "casts");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + newCast.CastName + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);

                if (files.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        files.CopyTo(stream);
                    }
                }

                newCast.CastImage = "/uploads/images/casts/" + fileName;
            }
            await _unitOfWork.CastRepository.Add(newCast);
            await _unitOfWork.Save();

            return _mapper.Map<CastDTO>(newCast);
        }

        public async Task Update(CastDTO castDTO, IFormFile file)
        {
            var cast = await _unitOfWork.CastRepository.GetById(castDTO.Id);
            if (cast == null)
            {
                // Handle not found
                throw new ArgumentException("Cast not found");
            }

            // Update cast properties
            cast.CastName = castDTO.CastName;

            if (file != null && file.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "casts");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + cast.CastName + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Update cast image path
                cast.CastImage = "/uploads/images/casts/" + fileName;
            }

            await _unitOfWork.CastRepository.Update(cast);
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
