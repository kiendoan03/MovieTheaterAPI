using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;
using System.Security.Claims;
using MovieTheaterAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        //[Authorize(Roles = "Staff, Manager")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customers = await _customerService.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Manager, Customer")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        //[Route("registration")]
        public async Task<ActionResult<CustomerDTO>> Registration([FromForm]CustomerDTO customer)
        {
            var newCustomer = await _customerService.Register(customer);
            return Ok(newCustomer);
        }

        //[HttpGet]
        //[Route("login")]
        //public async Task<ActionResult<CustomerDTO>> Login(string username, string password)
        //{
        //    var customer = await _customerService.Login(username, password);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(customer);
        //}

        [HttpPut("{id}")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> PutCustomer([FromForm] CustomerDTO customer,int id, IFormFile? file)
        {
            try
            {
                if (id != customer.Id)
                {
                    return BadRequest();
                }
                await _customerService.UpdateCustomer(customer, id, file);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Id mismatch");
            }
        }   

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomer(id);
            return NoContent();
        }
    }


    /* [Route("api/[controller]")]
     [ApiController]
     public class CustomersController : ControllerBase
     {
         private readonly IUnitOfWork _unitOfWork;
         private readonly IMapper _mapper;

         public CustomersController(IMapper mapper, IUnitOfWork unitOfWork)
         {
             _mapper = mapper;
             _unitOfWork = unitOfWork;
         }

         // GET: api/Customers
         [HttpGet]
         public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
         {
             var customers = await _unitOfWork.CustomerRepository.GetAll();
             return _mapper.Map<List<CustomerDTO>>(customers);
         }

         // GET: api/Customers/5
         [HttpGet("{id}")]
         public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
         {
             var customer = await _unitOfWork.CustomerRepository.GetById(id);

             if (customer == null)
             {
                 return NotFound();
             }

             return _mapper.Map<CustomerDTO>(customer);
         }

         // PUT: api/Customers/5
         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
         [HttpPut("{id}")]
         public async Task<IActionResult> PutCustomer(int id, CustomerDTO customer)
         {
             if (id != customer.Id)
             {
                 return BadRequest();
             }
             var updateCustomer = _mapper.Map<Customer>(customer);

             await _unitOfWork.CustomerRepository.Update(updateCustomer);

             try
             {
                 await _unitOfWork.Save();
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (CustomerExists(id))
                 {
                     return NotFound();
                 }
                 else
                 {
                     throw;
                 }
             }

             return NoContent();
         }

         // POST: api/Customers
         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
         [HttpPost]
         [Route("registration")]
         public async Task<ActionResult<CustomerDTO>> Registration(CustomerDTO customer)
         {
             var newCustomer = _mapper.Map<Customer>(customer);

             await _unitOfWork.CustomerRepository.Add(newCustomer);
             await _unitOfWork.Save();

             return CreatedAtAction("GetCustomer", new { id = newCustomer.Id }, customer);
         }

         [HttpGet]
         [Route("login")]
         public async Task<ActionResult<CustomerDTO>> Login(string Username, string password)
         {
             var customer = await _unitOfWork.CustomerRepository.Login(Username, password);
             if (customer == null)
             {
                 return NotFound();
             }
             var token = GenerateJwtToken(customer);
             var cusDTO = _mapper.Map<CustomerDTO>(customer);
             cusDTO.Token = token;
             return Ok(cusDTO);
         }

         // DELETE: api/Customers/5
         [HttpDelete("{id}")]
         public async Task<IActionResult> DeleteCustomer(int id)
         {
             var customer = await _unitOfWork.CustomerRepository.GetById(id);
             if (customer == null)
             {
                 return NotFound();
             }

             await _unitOfWork.CustomerRepository.Delete(customer);
             await _unitOfWork.Save();

             return NoContent();
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

         private bool CustomerExists(int id)
         {
             return _unitOfWork.CustomerRepository.IsExists(id).Result;
         }
     }*/
}
