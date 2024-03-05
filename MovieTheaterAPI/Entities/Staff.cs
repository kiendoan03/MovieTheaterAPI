namespace MovieTheaterAPI.Entities
{
    public class Staff : User
    {
        //public int StaffRole { get; set; } 
        public ICollection<Ticket>? Tickets { get; } = new List<Ticket>();
    }
}
