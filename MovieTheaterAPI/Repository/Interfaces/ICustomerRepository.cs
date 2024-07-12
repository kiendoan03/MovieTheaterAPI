using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        //Task<Customer> Login(string Username, string password);
        Task<int> CountTicketsBought(int id);
    }
}
