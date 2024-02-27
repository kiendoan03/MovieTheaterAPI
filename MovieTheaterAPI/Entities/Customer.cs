namespace MovieTheaterAPI.Entities
{
    public class Customer : User
    {
        public ICollection<Ticket>? Tickets { get; set; } = new List<Ticket>();
    }
}
