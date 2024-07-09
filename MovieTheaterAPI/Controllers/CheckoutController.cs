namespace MovieTheaterAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using MovieTheaterAPI.DTOs;
using System.Linq;

public class CheckoutController : Controller
{
    private readonly PayOS _payOS;


    public CheckoutController(PayOS payOS)
    {
        _payOS = payOS;

    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        return View("index");
    }
    [HttpGet("/cancel")]
    public IActionResult Cancel()
    {
        return View("cancel");
    }
    [HttpGet("/success")]
    public IActionResult Success()
    {
        return View("success");
    }
    [HttpPost("/create-payment-link")]
    public async Task<IActionResult> Checkout([FromBody] PaymentInformation paymentInformation)
    {
        try
        {
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            //ItemData item = new ItemData("Vé xem phim", 1, 2000);
            //List<ItemData> items = new List<ItemData>();
            //items.Add(item);
            List<ItemData> items = paymentInformation.Tickets.Select(ticket =>
            new ItemData("Vé xem phim", 1 , (int)ticket.FinalPrice)).ToList();
            //PaymentData paymentData = new PaymentData(orderCode, paymentInformation.TotalPrice, "Thanh toán vé xem phim", items, "http://localhost:5173/bookingTicket/13", "http://localhost:5173/bookingTicket/13");
            PaymentData paymentData = new PaymentData(orderCode,
                                          paymentInformation.TotalPrice,
                                          "Thanh toán vé xem phim",
                                          items,
                                          $"http://localhost:5173/bookingTicket/{paymentInformation.ScheduleId}",
                                          $"http://localhost:5173/bookingTicket/{paymentInformation.ScheduleId}");
            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            //return Redirect(createPayment.checkoutUrl);
            return Ok(new { checkoutUrl = createPayment.checkoutUrl });
        }
        catch (System.Exception exception)
        {
            Console.WriteLine(exception);
            //return Redirect("https://localhost:7013/");
            return StatusCode(500, "Internal server error");
        }
    }
}
