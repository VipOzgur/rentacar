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
        //public async Task<string?> ImageSaveDefaultAsync(IFormFile formFile , string? name)
        //{
        //    if (formFile == null || formFile.Length == 0)
        //    {
        //        return null;
        //    }
        //    string imageExtension = Path.GetExtension(formFile.FileName);
        //    string imageName = DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm-ss") + ".webp";
        //    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagess", imageName);
        //    ////ImageFile dolu ise ekliyoruz
        //    //imageExtension = Path.GetExtension(formFile.FileName);
        //    //imageName = name +DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm-ss") + imageExtension;
        //    //imagePath = Path.Combine("../Rentacar/wwwroot/imagess/" + imageName);
        //    await using (var fileStream = new FileStream(imagePath, FileMode.Create))
        //    {
        //        await formFile.CopyToAsync(fileStream);
        //    }

        //    return "/imagess/" + imageName;
        //}
        public async Task<string?> ImageSaveDefaultAsync(IFormFile formFile, string? name)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return null;
            }

            string imageExtension = Path.GetExtension(formFile.FileName);
            string imageName = (name ?? string.Empty) + DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm-ss") + imageExtension;
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagess", imageName);

            await using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return "/imagess/" + imageName;
        }


        public string? ImageSaveAsWebPAsync(IFormFile formFile, string? name)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return null;
            }


                string imageName = DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm-ss") + name + ".webp";
            try
            {

                // Görüntüyü okuyun
                var image = Image.Load(formFile.OpenReadStream());

                string imageExtension = Path.GetExtension(formFile.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagess", imageName);
                // WebP formatında kaydet
                var options = new WebpEncoder
                {
                    Quality = 100 // Kaliteyi belirleyebilirsiniz (0-100)
                };
                image.SaveAsync(imagePath, options);

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
        public bool DeleteImage(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
            {
                return false; // Geçersiz parametre
            }

            try
            {
                // Resmin tam yolunu oluştur
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageName);
                // Dosyanın var olup olmadığını kontrol et
                if (File.Exists(imagePath))
                {
                    // Dosyayı sil
                    File.Delete(imagePath);
                    Console.WriteLine($"Image deleted: {imagePath}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Image not found: {imagePath}");
                    return false; // Dosya bulunamadı
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting image: {ex.Message}");
                return false; // Hata durumunda
            }
        }
        public bool DeleteImageTwo(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return false; // Geçersiz parametre
            }

            try
            {
                // Girilen yolun başındaki "/imagess/" kısmını temizle ve tam dosya yolunu oluştur
                string fileName = imagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                // Dosyanın var olup olmadığını kontrol et
                if (File.Exists(fullPath))
                {
                    // Dosyayı sil
                    File.Delete(fullPath);
                    Console.WriteLine($"Image deleted: {fullPath}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Image not found: {fullPath}");
                    return false; // Dosya bulunamadı
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting image: {ex.Message}");
                return false; // Hata durumunda
            }
        }

    }
}
