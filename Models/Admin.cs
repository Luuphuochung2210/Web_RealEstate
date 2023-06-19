using System;
using System.Collections.Generic;

namespace Web_RealEstate.Models;

public partial class Admin
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PassWord { get; set; } = null!;

    public string? Email { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
