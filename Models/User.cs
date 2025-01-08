using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class User
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string? Telefon { get; set; }

    public string? Adres { get; set; }

    public string? Eposta { get; set; }

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public byte[]? IsActive { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Rezervasyon> Rezervasyons { get; set; } = new List<Rezervasyon>();

    public virtual Role Role { get; set; } = null!;
}
