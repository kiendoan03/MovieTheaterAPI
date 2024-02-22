namespace MovieTheaterAPI.Entities
{
    public class Staff
    {
        public int Id { get; set; }
        public string StaffName { get; set; } = null!;
        public string StaffImage { get; set; } = null!;
        public string StaffRole { get; set; } = null!;
        public string StaffEmail { get; set; } = null!;
        public string StaffPhone { get; set; } = null!;
        public string StaffUsername { get; set; } = null!;
        public string StaffPassword { get; set; } = null!;
        public DateOnly? StaffDOB { get; set; }
        public string StaffAddress { get; set; } = null!;
        public string StaffImg { get; set; } = null!;
        public ICollection<Ticket>? Tickets { get; } = new List<Ticket>();
    }
}
