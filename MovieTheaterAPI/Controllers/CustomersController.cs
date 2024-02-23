using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO customer)
        {
            var newCustomer = _mapper.Map<Customer>(customer);
           
            await _unitOfWork.CustomerRepository.Add(newCustomer);
            await _unitOfWork.Save();

            return CreatedAtAction("GetCustomer", new { id = newCustomer.Id }, newCustomer);
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

        private bool CustomerExists(int id)
        {
            return _unitOfWork.CustomerRepository.IsExists(id).Result;
        }
    }
}
