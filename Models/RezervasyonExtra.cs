using System.ComponentModel.DataAnnotations.Schema;

namespace Rentacar.Models
{
    [NotMapped]
    public class RezervasyonExtra : BaseEntity
    {
        public int RezervasyonId { get; set; }
        public Rezervasyon Rezervasyon { get; set; } = null!;

        public int ExtraId { get; set; }
        public Extralar Extra { get; set; } = null!;
    }

}
