using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Options;
using Rentacar.Models;
using Iyzipay;
using System.Globalization;

namespace Rentacar.Services
{
    public class PaymentService
    {
        private readonly IyzicoOptions _options;

        public PaymentService(IOptions<IyzicoOptions> options)
        {
            _options = options.Value;
        }
        public async Task<CheckoutForm> RetrieveCheckoutForm(string token)
        {
            var options = new Iyzipay.Options
            {
                ApiKey = _options.ApiKey,
                SecretKey = _options.SecretKey,
                BaseUrl = _options.BaseUrl
            };

            var request = new RetrieveCheckoutFormRequest
            {
                Token = token
            };

            return await CheckoutForm.Retrieve(request, options);
        }

        public async Task<string> CreateCheckoutForm(decimal price, string userEmail)
        {
            Iyzipay.Options iyzicoOptions = new Iyzipay.Options
            {
                ApiKey = _options.ApiKey,
                SecretKey = _options.SecretKey,
                BaseUrl = _options.BaseUrl
            };

            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = Guid.NewGuid().ToString(),
                Price = price.ToString("0.00",CultureInfo.InvariantCulture),
                PaidPrice = price.ToString("0.00", CultureInfo.InvariantCulture),
                Currency = Currency.TRY.ToString(),
                BasketId = "B67832",
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                CallbackUrl = "https://localhost:44366/Payment/Callback"
            };

            request.Buyer = new Buyer
            {
                Id = "BY789",
                Name = "Rıdvan",
                Surname = "Çam",
                GsmNumber = "+905350000000",
                Email = userEmail,
                IdentityNumber = "74300864791",
                RegistrationAddress = "Adres",
                Ip = "85.34.78.112",
                City = "İstanbul",
                Country = "Türkiye"
            };

            request.ShippingAddress = new Address
            {
                ContactName = "Rıdvan Çam",
                City = "İstanbul",
                Country = "Türkiye",
                Description = "Kargo adresi"
            };

            request.BillingAddress = request.ShippingAddress;

            request.BasketItems = new List<BasketItem>
        {
            new BasketItem
            {
                Id = "BI101",
                Name = "Araç Kiralama",
                Category1 = "RentACar",
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = price.ToString("0.00",CultureInfo.InvariantCulture)
            }
        };

            CheckoutFormInitialize checkoutFormInitialize = await CheckoutFormInitialize.Create(request, iyzicoOptions);
            // Hata kontrolü
            if (checkoutFormInitialize.Status != "success")
            {
                Console.WriteLine("İyzico hata mesajı: " + checkoutFormInitialize.ErrorMessage);
                Console.WriteLine("Hata detayları: " + checkoutFormInitialize.Signature);
                return $"<p style='color:red'>Ödeme formu oluşturulamadı: {checkoutFormInitialize.ErrorMessage}</p>";
            }

            return checkoutFormInitialize.CheckoutFormContent; // bu HTML formu döner
        }
    }
}
