using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Lokasyonlar : BaseEntity
{
    public string Ad { get; set; } = null!;

    public int? Kordinatlar { get; set; }

    public int? Durum { get; set; }

    public int? Fiyat { get; set; }

    public virtual ICollection<Rezervasyon> RezervasyonAlisLokasyons { get; set; } = new List<Rezervasyon>();

    public virtual ICollection<Rezervasyon> RezervasyonTeslimLokasyons { get; set; } = new List<Rezervasyon>();
}
