using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        //Task<Customer> Login(string Username, string password);
        Task<int> CountTicketsBought(int id);
        //CheckDuplicateCustomer
        Task<bool> CheckDuplicateCustomer(string username, string email);
        //CheckDuplicateCustomerExcept
        Task<bool> CheckDuplicateCustomerExcept(string username, string email, int id);
    }
}
