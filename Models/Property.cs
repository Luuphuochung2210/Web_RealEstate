using System;
using System.Collections.Generic;

namespace Web_RealEstate.Models;

public partial class Property
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string DienTich { get; set; } = null!;

    public string Price { get; set; } = null!;

    public int CategoryId { get; set; }

    public int LocationId { get; set; }

    public int ChuDauTuId { get; set; }

    public string? Image { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ChuDauTu ChuDauTu { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
