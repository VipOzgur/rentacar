using System.ComponentModel.DataAnnotations.Schema;

namespace Rentacar.Models
{
    [NotMapped]
    public class IyzicoOptions
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string BaseUrl { get; set; }
    }

}
