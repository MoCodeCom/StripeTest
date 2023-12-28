using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
//using Stripe.BillingPortal;
using stripeTest.Models;

namespace stripeTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly StripeSettings _stripeSettings;
    public string SessionId { get; set; }

    public HomeController(ILogger<HomeController> logger,
        IOptions<StripeSettings> stripeSettings
        )
    {
        _logger = logger;
        _stripeSettings = stripeSettings.Value;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult CreateCheckoutSession(string amount)
    {
        var currancy = "usd";
        var successUrl = "https://localhost:7214/Home/success";
        var cancelUrl = "https://localhost:7214/Home/cancel";
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
            {
                "card"
            },

            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currancy,
                        UnitAmount = Convert.ToInt32(amount)*100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {

                            Name = "Product Name",
                            Description = "Product Description",

                        }
                    },
                    Quantity = 1
                }
            },

            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl
        };

        var service = new SessionService();
        var session = service.Create(options);
        SessionId = session.Id;

        return Redirect(session.Url);
    }

    public async Task<IActionResult> success()
    {
        return View("Index");
    }

    public IActionResult cancel()
    {
        return View("Index");
    }
}

