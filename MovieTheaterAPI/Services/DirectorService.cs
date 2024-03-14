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

        public async Task<DirectorDTO> CreateDirector(DirectorDTO director, IFormFile file)
        {
            var newDirector = _mapper.Map<Director>(director);
            if(file.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "directors");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + newDirector.DirectorName + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);

                if (file.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                newDirector.directorImage = "/uploads/images/directors/" + fileName;
            }
            await _unitOfWork.DirectorRepository.Add(newDirector);
            await _unitOfWork.Save();
            return _mapper.Map<DirectorDTO>(newDirector);
        }

        public async Task UpdateDirector( DirectorDTO director,int id, IFormFile? file)
        {
            if (id != director.Id)
            {
                throw new ArgumentException("Id mismatch");
            }

            //var updatedDirector = _mapper.Map<Director>(director);

            var oldDirector = await _unitOfWork.DirectorRepository.GetById(id);
            
            if (file != null && file.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "directors");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + director.DirectorName + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);
                
                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                director.directorImage = "/uploads/images/directors/" + fileName;
            }
            else
            {
                director.directorImage = oldDirector.directorImage;
            }

            var updateDirector = _mapper.Map(director, oldDirector);

            await _unitOfWork.DirectorRepository.Update(updateDirector);
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
