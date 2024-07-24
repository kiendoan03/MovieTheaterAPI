﻿using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomers();
        Task<CustomerDTO> GetCustomerById(int id);
        Task<CustomerDTO> Register(CustomerDTO customer);
        //Task<CustomerDTO> Login(string username, string password);
        Task UpdateCustomer(CustomerDTO customer, int id, IFormFile? file);
        Task DeleteCustomer(int id);
        Task<int> CountTicketsBought(int id);
        //CheckDuplicateCustomer
        Task<bool> CheckDuplicateCustomer(string username, string email);
        //CheckDuplicateCustomerExcept
        Task<bool> CheckDuplicateCustomerExcept(string username, string email, int id);
    }

}
