using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;
using MovieTheaterAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieTheaterAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAll();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetCustomerById(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetById(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<CustomerDTO> Register(CustomerDTO customer)
        {
            var newCustomer = _mapper.Map<Customer>(customer);
            await _unitOfWork.CustomerRepository.Add(newCustomer);
            await _unitOfWork.Save();
            return _mapper.Map<CustomerDTO>(newCustomer);
        }

        public async Task<CustomerDTO> Login(string username, string password)
        {
            var customer = await _unitOfWork.CustomerRepository.Login(username, password);
            if (customer == null)
            {
                return null;
            }

            var token = GenerateJwtToken(customer);
            var cusDTO = _mapper.Map<CustomerDTO>(customer);
            cusDTO.Token = token;
            return cusDTO;
        }

        public async Task UpdateCustomer(int id, CustomerDTO customer)
        {
            if (id != customer.Id)
            {
                throw new ArgumentException("Id mismatch");
            }

            var updatedCustomer = _mapper.Map<Customer>(customer);
            await _unitOfWork.CustomerRepository.Update(updatedCustomer);
            await _unitOfWork.Save();
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetById(id);
            await _unitOfWork.CustomerRepository.Delete(customer);
            await _unitOfWork.Save();
        }

        private string GenerateJwtToken(Customer customer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("super secret key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", customer.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
