﻿using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.ViewModels;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IEmailService
    {
        //Task SendEmailAsync(string to, string subject, string body);
        Task SendEmailAsync(string to, string subject, EmailViewModel model);
        Task SendTestEmail(string to, string subject, EmailViewModel model);
        Task SendPaymentConfirmationEmail(string to, PaymentSuccessPayload paymentSuccessPayload);
    }
}