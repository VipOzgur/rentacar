using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Yorumlar : BaseEntity
{
    public string Yorum { get; set; } = null!;

    public int? UserId { get; set; }

    public int? AracId { get; set; }

    public int? Durum { get; set; }

    public virtual Araclar? Arac { get; set; }

    public virtual User? User { get; set; }
}
