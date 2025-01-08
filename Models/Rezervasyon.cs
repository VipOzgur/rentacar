﻿using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Rezervasyon
{
    public int Id { get; set; }

    public int AracId { get; set; }

    public int UserId { get; set; }

    public string? StartDate { get; set; }

    public string? FinishDate { get; set; }

    public int? Durum { get; set; }

    public int? Onay { get; set; }

    public virtual Araclar Arac { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
