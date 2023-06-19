using System;
using System.Collections.Generic;

namespace Web_RealEstate.Models;

public partial class Location
{
    /// <summary>
    /// Mã tỉnh thành
    /// </summary>
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    /// <summary>
    /// Mã quận huyện
    /// </summary>
    public int? ParentId { get; set; }

    public virtual ICollection<Location> InverseParent { get; set; } = new List<Location>();

    public virtual Location? Parent { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
