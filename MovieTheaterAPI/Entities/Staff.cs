namespace MovieTheaterAPI.Entities
{
    public class Staff : User
    {
        public string StaffRole { get; set; } = null!;
        public ICollection<Ticket>? Tickets { get; } = new List<Ticket>();
    }
}
