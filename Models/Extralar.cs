using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Extralar
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public int? Fiyat { get; set; }

    public int? Durum { get; set; }
}
