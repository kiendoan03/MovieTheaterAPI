using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
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
        //private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        //private readonly SignInManager<User> _signinManager;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_passwordHasher = passwordHasher;
            _userManager = userManager;
            _tokenService = tokenService;
            //_signinManager = signInManager;
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
            /*//newCustomer.PasswordHash = _passwordHasher.HashPassword(newCustomer, customer.PasswordHash);
            //newCustomer.SecurityStamp = System.Guid.NewGuid().ToString();
            //newCustomer.NormalizedEmail = newCustomer.Email.ToUpper();
            //newCustomer.NormalizedUserName = newCustomer.UserName.ToUpper();
            //await _unitOfWork.CustomerRepository.Add(newCustomer);
            //await _unitOfWork.Save();
            //return _mapper.Map<CustomerDTO>(newCustomer);*/
            newCustomer.Image = "/uploads/images/customers/avt-default.png";
            var createdUser = await _userManager.CreateAsync(newCustomer, customer.PasswordHash);
            if(!createdUser.Succeeded)
            {
                throw new AccessViolationException("User creation failed");
            }
            else
            {
               var role = await _userManager.AddToRoleAsync(newCustomer, "Customer");
                if (!role.Succeeded)
                {
                    throw new AccessViolationException("Role creation failed");
                }
                else
                {
                    var cusDTO = _mapper.Map<CustomerDTO>(newCustomer);
                    cusDTO.Token = await _tokenService.CreateTokenAsync(newCustomer);
                    return cusDTO;
                }
            }
        }

        //public async Task<CustomerDTO> Login(string username, string password)
        //{
        //    var customer = await _unitOfWork.CustomerRepository.Login(username, password);
        //    if (customer == null)
        //    {
        //        return null;
        //    }

        //    var token = GenerateJwtToken(customer);
        //    var cusDTO = _mapper.Map<CustomerDTO>(customer);
        //    cusDTO.Token = token;
        //    return cusDTO;
        //}

        //public async Task UpdateCustomer(CustomerDTO customer, int id, IFormFile? file)
        //{
        //    if (id != customer.Id)
        //    {
        //        throw new ArgumentException("Id mismatch");
        //    }

        //    var updatedCustomer = _mapper.Map<Customer>(customer);

        //    if (file != null && file.Length > 0)
        //    {
        //        var folderName = Path.Combine("wwwroot", "uploads", "images", "customers");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        var fileName = Guid.NewGuid().ToString() + "_" + updatedCustomer.Name + ".png";
        //        var fullPath = Path.Combine(pathToSave, fileName);

        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //        }

        //        updatedCustomer.Image = "/uploads/images/customers/" + fileName;
        //    }
        //    else
        //    {
        //        updatedCustomer.Image = updatedCustomer.Image;
        //    }

        //    await _unitOfWork.CustomerRepository.Update(updatedCustomer);
        //    await _unitOfWork.Save();
        //}

        public async Task UpdateCustomer(CustomerDTO customer, int id, IFormFile? file)
        {
            if (id != customer.Id)
              {
               throw new ArgumentException("Id mismatch");
              }
            var oldCus = await _unitOfWork.CustomerRepository.GetById(id);

            if (file != null && file.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "customers");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + customer.Username + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                customer.Image = "/uploads/images/customers/" + fileName;
            }
            else
            {
                customer.Image = oldCus.Image;
            }


            if (string.IsNullOrEmpty(customer.PasswordHash))
            {
                // If no new password provided, keep the old password
                customer.PasswordHash = oldCus.PasswordHash;
                _mapper.Map(customer, oldCus);
            }
            else
            {
                _mapper.Map(customer, oldCus);
                // If a new password is provided, hash it
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(oldCus, customer.PasswordHash);
                // Update the staff's password hash
                oldCus.PasswordHash = newPasswordHash;
            }

            // Update the user
            await _userManager.UpdateAsync(oldCus);
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetById(id);
            await _unitOfWork.CustomerRepository.Delete(customer);
            await _unitOfWork.Save();
        }

        public async Task<int> CountTicketsBought(int id)
        {
            return await _unitOfWork.CustomerRepository.CountTicketsBought(id);
        }

        public async Task<bool> CheckDuplicateCustomer(string username, string email)
        {
            return await _unitOfWork.CustomerRepository.CheckDuplicateCustomer(username, email);
        }

        public async Task<bool> CheckDuplicateCustomerExcept(string username, string email, int id)
        {
            return await _unitOfWork.CustomerRepository.CheckDuplicateCustomerExcept(username, email, id);
        }

        //private string GenerateJwtToken(Customer customer)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes("super secret key");
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[] { new Claim("id", customer.Id.ToString()) }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
    }

}
