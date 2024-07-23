namespace MovieTheaterAPI.DTOs
{
    public class EmailOtpVerifyInputDto
    {
        public string StringEncrypted { get; set; }
        public string Otp { get; set; }
    }
}
