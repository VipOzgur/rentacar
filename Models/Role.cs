using System;
using System.Collections.Generic;

namespace Rentacar.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
