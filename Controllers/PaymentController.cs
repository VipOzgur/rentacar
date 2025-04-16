using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Rentacar.Services;

namespace Rentacar.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Checkout()
        {
            var formHtml = await _paymentService.CreateCheckoutForm(400, "deneme@example.com");
            ViewBag.CheckoutForm = formHtml;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Callback()
        {
            var token = Request.Form["token"];
            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Message = "Ödeme sırasında bir hata oluştu (token alınamadı).";
                return View();
            }

            var checkoutForm = await _paymentService.RetrieveCheckoutForm(token);

            if (checkoutForm.Status == "success")
            {
                ViewBag.Message = "Ödeme başarılı!";
                // Burada rezervasyon kaydı yapılabilir
            }
            else
            {
                ViewBag.Message = "Ödeme başarısız: " + checkoutForm.ErrorMessage;
            }

            return View();
        }


    }
}
