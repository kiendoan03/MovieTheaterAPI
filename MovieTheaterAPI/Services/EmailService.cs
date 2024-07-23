using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using MovieTheaterAPI.Services.Interfaces;
using MovieTheaterAPI.ViewModels;
using MovieTheaterAPI.DTOs;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography;
using System.Text;

namespace MovieTheaterAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IViewRenderService _viewRenderService;

        public EmailService(IConfiguration configuration, IViewRenderService viewRenderService)
        {
            _configuration = configuration;
            _viewRenderService = viewRenderService;
        }

        public async Task SendEmailOtp(string to, string subject, EmailViewModel model)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            // Render the email body using the view
            var body = await _viewRenderService.RenderToStringAsync("EmailBody", model);
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        public async Task SendEmailAsync(string to, string subject, EmailViewModel model)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            // Render the email body using the view
            var body = await _viewRenderService.RenderToStringAsync("Email/EmailBody", model);
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendPaymentConfirmationEmail(string to, PaymentSuccessPayload payment)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = "Payment Confirmation";
            var body = await _viewRenderService.RenderToStringAsync("Email/EmailTicket", payment);
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task<string> GenerateEmailOtp(string toEmail)
        {
            var _random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var randomString = new string(Enumerable.Repeat(chars, 6)
                 .Select(s => s[_random.Next(s.Length)]).ToArray());
            var randomStringEncrypted = await Encrypted(randomString);
            var emailViewModel = new EmailViewModel
            {
                Title = "Email OTP",
                Content = $"{randomString}"
            };
            await SendEmailOtp(toEmail, emailViewModel.Title, emailViewModel);
            return randomStringEncrypted;
        }

        public async Task<string> Encrypted(string randomString)
        {
            var randomStringEncrypted = "";
            string EncryptionKey = "";
            byte[] clearBytes = Encoding.Unicode.GetBytes(randomString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    randomStringEncrypted = Convert.ToHexString(ms.ToArray());
                }
            }
            return randomStringEncrypted;
        }

        public async Task<bool> VerifyEmailOtp(EmailOtpVerifyInputDto input)
        {
            var otpDecrypt = await Encrypted(input.Otp);
            if (otpDecrypt.Equals(input.StringEncrypted)) return true;

            return false;
        }

        //public async Task SendPaymentConfirmationEmail(string to, PaymentSuccessPayload paymentSuccess)
        //{
        //    foreach (var ticket in paymentSuccess.Tickets)
        //    {
        //        // Generate HTML content from view
        //        var htmlContent = await _viewRenderService.RenderToStringAsync("QRCode/QRCode", ticket);

        //        // Generate QR code for the HTML content
        //        var qrCodeGenerator = new QRCodeGenerator();
        //        var qrCodeData = qrCodeGenerator.CreateQrCode(htmlContent, QRCodeGenerator.ECCLevel.Q);
        //        //var qrCode = new PngByteQRCode(qrCodeData);
        //        //var qrCodeImage = qrCode.GetGraphic(6);
        //        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        //        {
        //            byte[] qrCodeImage = qrCode.GetGraphic(20);
        //            var emailViewModel = new EmailViewModel
        //            {
        //                Title = "Payment Confirmation",
        //                Content = $"<p>Thank you for your payment. Your order details are as follows:<br>" +
        //                     $"Total Amount: {paymentSuccess.TotalPrice} VND<br>" +
        //                     $"Tickets:<br>" +
        //                     $"- Seat: {ticket.Seats.Row}{ticket.Seats.Column}, Price: {ticket.FinalPrice} VND<br>" +
        //                     $"<img src=\"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}\" alt=\"QR Code\" width=\"150\" height=\"150\"></p>"
        //            };
        //            await SendEmailAsync(to, emailViewModel.Title, emailViewModel);
        //        }
        //        // Save the QR code image to a file if needed
        //        //var qrCodeFilePath = Path.Combine("wwwroot", "uploads", "qrcodes", $"ticket_{ticket.Seats.Row}{ticket.Seats.Column}.png");
        //        //using (var fileStream = new FileStream(qrCodeFilePath, FileMode.Create))
        //        //{
        //        //    qrCodeImage.Save(fileStream, ImageFormat.Png);
        //        //}

        //        // Add the QR code image to the email content
        //    }
        //}

        //public async Task SendPaymentConfirmationEmail(string to, PaymentSuccessPayload paymentSuccess)
        //{
        //    var emailViewModel = new EmailViewModel
        //    {
        //        Title = "Payment Confirmation",
        //        Content = $"<p>Thank you for your payment. Your order details are as follows:\n" +
        //                  $"Total Amount: {paymentSuccess.TotalPrice}\n" +
        //                  $"Tickets:\n" +
        //                  $"- Movie: {paymentSuccess.Schedule.Movie.MovieName}"
        //    };

        //    foreach (var ticket in paymentSuccess.Tickets)
        //    {
        //        emailViewModel.Content += $"- Seat: {ticket.Seats.Row}{ticket.Seats.Column}, Price: {ticket.FinalPrice}</p>";
        //    }

        //    await SendEmailAsync(to, emailViewModel.Title, emailViewModel);
        //}

        //public async Task SendPaymentConfirmationEmail(string to, PaymentSuccessPayload paymentSuccess)
        //{
        //    var emailViewModel = new EmailViewModel { Title = "Payment Confirmation", Content = $"<p>Thank you for your payment. Your order details are as follows:\n" + $"Total Amount: {paymentSuccess.TotalPrice}\n" + $"Tickets:\n" }; 
        //    foreach (var ticket in paymentSuccess.Tickets)
        //    {
        //        emailViewModel.Content += $"- Seat: {ticket.Seats.Row}{ticket.Seats.Column}, Price: {ticket.FinalPrice}</p>";                
        //        // Generate QR code for each ticket
        //        var qrCodeGenerator = new QRCodeGenerator();                
        //        var qrCodeData = qrCodeGenerator.CreateQrCode($"Ticket: {ticket.Seats.Row}{ticket.Seats.Column}", QRCodeGenerator.ECCLevel.Q);                
        //        var qrCode = new QRCode(qrCodeData); 
        //        var qrCodeImage = qrCode.GetGraphic(6);
        //        // Save the QR code image to a file
        //        var qrCodeFilePath = Path.Combine("wwwroot", "uploads", "qrcodes", $"ticket_{ticket.Seats.Row}{ticket.Seats.Column}.png");                
        //        using (var fileStream = new FileStream(qrCodeFilePath, FileMode.Create))               
        //        {                    
        //            qrCodeImage.Save(fileStream, ImageFormat.Png);                
        //        }               
        //        // Add the QR code image to the email content
        //        emailViewModel.Content += $"<img src=\"{qrCodeFilePath}\" alt=\"QR Code\" width=\"150\" height=\"150\">";}           
        //        await SendEmailAsync(to, emailViewModel.Title, emailViewModel);        }
        //    }

    }
}
