using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Kaskolar
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public int? GunlukFiyat { get; set; }

    public int? SaatlikFiyat { get; set; }
}
