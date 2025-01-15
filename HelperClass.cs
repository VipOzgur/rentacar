using Rentacar.Models;
using System.Security.Cryptography;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
namespace Rentacar
{
    public class HelperClass
    {
        //string imageExtension = "";
        //string imageName = "";
        //string imagePath = "";
        //public async Task<string?> ImageSaveAsync(IFormFile formFile )
        //{
        //    if (formFile == null || formFile.Length == 0)
        //    {
        //        return null;
        //    }
        //    //ImageFile dolu ise ekliyoruz
        //    imageExtension = Path.GetExtension(formFile.FileName);
        //    imageName = DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm-ss") + imageExtension;
        //    imagePath = Path.Combine("../Rentacar/wwwroot/imagess/" + imageName);
        //    await using (var fileStream = new FileStream(imagePath, FileMode.Create))
        //    {
        //        await formFile.CopyToAsync(fileStream);
        //    }

        //    return "/imagess/" + imageName;
        //}


        public async Task<string?> ImageSaveAsWebPAsync(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return null;
            }

            string imageExtension = Path.GetExtension(formFile.FileName);
            string imageName = DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm-ss") + ".webp";
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagess", imageName);

            try
            {
                // Görüntüyü okuyun
                using (var image = await Image.LoadAsync(formFile.OpenReadStream()))
                {
                    // WebP formatında kaydet
                    var options = new WebpEncoder
                    {
                        Quality = 100 // Kaliteyi belirleyebilirsiniz (0-100)
                    };

                    await image.SaveAsync(imagePath, options);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yapabilirsiniz
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

            return "/imagess/" + imageName;
        }



        public string? Hash(string input)
        {
            //Şifre hashleme kodları
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
