using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Kaskolar : BaseEntity
{
    public string Ad { get; set; } = null!;

    public int? GunlukFiyat { get; set; }

    public int? SaatlikFiyat { get; set; }

    public virtual ICollection<Rezervasyon> Rezervasyonlar { get; set; }=new List<Rezervasyon>();
}
