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

        public async Task SendTestEmail(string to, string subject, EmailViewModel model)
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
