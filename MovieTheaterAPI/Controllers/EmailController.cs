using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Services.Interfaces;
using MovieTheaterAPI.ViewModels;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost]
        public async Task<IActionResult> SendTestEmail() {
            var emailViewModel = new EmailViewModel { Title = "Test Email", Content = "<h1>This is a test email</h1>" };
            //await _emailService.SendEmailAsync("kiendoan8521349@gmail.com", "Test Email", "<h1>This is test email</h1>");
            await _emailService.SendEmailAsync("kiendoan8521349@gmail.com", emailViewModel.Title, emailViewModel);
            return Ok(); 
        }

        [HttpPost]
        //[Route("generate-email-otp/{to}")]
        [AllowAnonymous]

        public async Task<string> GenerateEmailOtp( string to)
        {
            var response = await _emailService.GenerateEmailOtp(to);
            return response;

        }
        [HttpPost]
        //[Route("verify-email-otp")]
        [AllowAnonymous]

        public async Task<bool> VerifyEmailOtp(EmailOtpVerifyInputDto input)
        {
            var response = await _emailService.VerifyEmailOtp(input);
            return response;
        }
    }
}
