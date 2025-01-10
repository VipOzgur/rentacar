using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Blog : BaseEntity
{

    public int? Tip { get; set; }

    public int AdminId { get; set; }

    public string? Icerik { get; set; }

    public byte[]? IsActive { get; set; }

    public virtual User Admin { get; set; } = null!;
}
