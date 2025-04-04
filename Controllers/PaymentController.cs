using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    [HttpPost("pay")]
    public async Task<IActionResult> Pay()
    {
        Options options = new Options
        {
            ApiKey = "sandbox-2FUhsidyfuScSKndWLDqmqIEyLOpckjP",  // BURAYA KENDİ API ANAHTARLARINI KOY
            SecretKey = "sandbox-JYYuNFbfYNdymlDrNEnGeo6ZlAnpTYnO",
            BaseUrl = "https://sandbox-api.iyzipay.com"
        };

        CreatePaymentRequest request = new CreatePaymentRequest
        {
            Locale = Locale.TR.ToString(),
            ConversationId = "123456789",
            Price = "100.0",
            PaidPrice = "100.0",
            Currency = Currency.TRY.ToString(),
            Installment = 1,
            BasketId = "B67832",
            PaymentChannel = PaymentChannel.WEB.ToString(),
            PaymentGroup = PaymentGroup.PRODUCT.ToString(),
            PaymentCard = new PaymentCard
            {
                CardHolderName = "Buğra Kurnaz",
                CardNumber = "5528790000000008",
                ExpireMonth = "12",
                ExpireYear = "2030",
                Cvc = "123",
                RegisterCard = 0
            },
            BasketItems = new List<BasketItem>
            {
                new BasketItem
                {
                    Id = "BI101",
                    Name = "Rent A Car Hizmeti",
                    Category1 = "Araç Kiralama",
                    ItemType = BasketItemType.VIRTUAL.ToString(),
                    Price = "100.0"
                }
            },
            Buyer = new Buyer
            {
                Id = "BY789",
                Name = "Buğra",
                Surname = "Kurnaz",
                GsmNumber = "+905350000000",
                Email = "johndoe@example.com",
                IdentityNumber = "74300864791",
                RegistrationAddress = "Istanbul, Turkey",
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34000"
            },
            //ShippingAddress = new Address
            //{
            //    ContactName = "Buğra Kurnaz",
            //    City = "Istanbul",
            //    Country = "Turkey",
            //    Description = "Istanbul, Turkey"
            //},
            BillingAddress = new Address
            {
                ContactName = "Buğra Kurnaz",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Istanbul, Turkey"
            }
        };

        Payment payment = await Payment.Create(request, options);

        if (payment.Status == "success")
        {
            return Ok(new { message = "Ödeme Başarılı!", payment });
        }
        else
        {
            return BadRequest(new { message = "Ödeme Başarısız!", error = payment.ErrorMessage });
        }
    }
}