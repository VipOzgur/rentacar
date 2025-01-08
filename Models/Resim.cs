using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Resim
{
    public int Id { get; set; }

    public int AracId { get; set; }

    public string Url { get; set; } = null!;

    public string? AltG { get; set; }

    public virtual Araclar Arac { get; set; } = null!;
}
