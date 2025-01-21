using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rentacar.Models;

public partial class Araclar : BaseEntity
{
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    [NotMapped]
    public List<IFormFile>? ImageFiles { get; set; }

    public string? Aciklama { get; set; }

    public string? Plaka { get; set; }

    public string? Marka { get; set; }

    public string? Model { get; set; }

    public byte[]? IsActive { get; set; }

    public string? Profil { get; set; }

    public string? Yakit { get; set; }

    public string? Vites { get; set; }

    public int? Fiyat { get; set; }

    public int? Durum { get; set; }

    public int? Depozito { get; set; }

    public int? Km { get; set; }

    public int? GoruntulemeSayisi { get; set; }

    public int? KiralanmaSayisi { get; set; }

    public string? MotorGucu { get; set; }

    public string? YakitKm { get; set; }

    public virtual ICollection<Resim> Resims { get; set; } = new List<Resim>();

    public virtual ICollection<Rezervasyon> Rezervasyons { get; set; } = new List<Rezervasyon>();

    public virtual ICollection<Yorumlar> Yorumlars { get; set; } = new List<Yorumlar>();
}
