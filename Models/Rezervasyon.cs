using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rentacar.Models;

public partial class Rezervasyon : BaseEntity
{
    [NotMapped]
    public List<int> SelectedExtralar { get; set; } = new();

    public int AracId { get; set; }

    public int UserId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public int? Durum { get; set; }

    public int? Onay { get; set; }

    public int? AlisLokasyonId { get; set; }

    public int? TeslimLokasyonId { get; set; }

    public int? KaskoId { get; set; }

    public int? Fiyat { get; set; }

    public string? Not { get; set; }

    public virtual Lokasyonlar? AlisLokasyon { get; set; }

    public virtual Araclar? Arac { get; set; } = null!;

    public virtual Lokasyonlar? TeslimLokasyon { get; set; }

    public virtual User? User { get; set; } = null!;

    public virtual ICollection<Extralar> Extralar { get; set; } = new List<Extralar>();
}
