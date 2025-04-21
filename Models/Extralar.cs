using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Extralar : BaseEntity

{
    public string Ad { get; set; } = null!;

    public int? Fiyat { get; set; }

    public int? Durum { get; set; }

    public virtual ICollection<Rezervasyon> Rezervasyons { get; set; } = new List<Rezervasyon>();
}
