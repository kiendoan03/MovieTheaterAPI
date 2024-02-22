namespace MovieTheaterAPI.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public string CustomerAddress { get; set; } = null!;
        public string CustomerUsername { get; set; } = null!;
        public string CustomerPassword { get; set; } = null!;
        public string CustomerImage { get; set; } = null!;
        public DateOnly? CustomerBirthdate { get; set; }
        public ICollection<Ticket>? Tickets { get; set; } = new List<Ticket>();
    }
}
