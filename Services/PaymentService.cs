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

        public async Task<string> CreateCheckoutForm(Rezervasyon rezervasyon)
        {
            string lokasyon;
            if (rezervasyon?.AlisLokasyon == null && rezervasyon?.TeslimLokasyon == null)
            {
                lokasyon = "Yozgat Ofis";
            }
            else
            {
                var alis = rezervasyon?.AlisLokasyon?.Ad ?? "Bilinmeyen Alış";
                var teslim = rezervasyon?.TeslimLokasyon?.Ad ?? "Bilinmeyen Teslim";
                lokasyon = $"{alis} => {teslim}";
            }
            Iyzipay.Options iyzicoOptions = new Iyzipay.Options
            {
                ApiKey = _options.ApiKey,
                SecretKey = _options.SecretKey,
                BaseUrl = _options.BaseUrl
            };

            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = rezervasyon.UserId+"-"+Guid.NewGuid().ToString(),
                Price = rezervasyon.Fiyat.Value.ToString("0.00",CultureInfo.InvariantCulture),
                PaidPrice = rezervasyon.Fiyat.Value.ToString("0.00", CultureInfo.InvariantCulture),
                Currency = Currency.TRY.ToString(),
                BasketId = "B67832",
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                CallbackUrl = "https://localhost:44366/Payment/Callback"
            };

            request.Buyer = new Buyer
            {
                Id = rezervasyon.User.Id.ToString(),
                Name = rezervasyon.User.Ad,
                Surname = rezervasyon.User.Soyad,
                GsmNumber = rezervasyon.User.Telefon,
                Email = rezervasyon.User.Eposta,
                IdentityNumber = rezervasyon.User.Id.ToString()+DateTime.Now.Year.ToString(),
                RegistrationAddress = rezervasyon.User.Adres??"Default Yozgat",
                Ip = "85.34.78.112",
                City = "Yozgat",
                Country = "Türkiye"
            };

            request.ShippingAddress = new Address
            {
                ContactName = "Onur Rentacar",
                City = "Yozgat",
                Country = "Türkiye",
                Description = lokasyon
            };

            request.BillingAddress = request.ShippingAddress;

            request.BasketItems = new List<BasketItem>
        {
            new BasketItem
            {
                Id = rezervasyon.AracId.ToString(),
                Name = (rezervasyon.Arac.Marka + " "+rezervasyon.Arac.Model )??"Araç Kiralama",
                Category1 = "RentACar",
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = rezervasyon.Fiyat.Value.ToString("0.00",CultureInfo.InvariantCulture)
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
