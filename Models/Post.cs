using System;
using System.Collections.Generic;

namespace Web_RealEstate.Models;

public partial class Post
{
    public int Id { get; set; }

    public int SellerId { get; set; }

    public int AdminId { get; set; }

    public int RealEstateId { get; set; }

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    /// <summary>
    /// 0: unPublic  1: Public
    /// </summary>
    public int Status { get; set; }

    public string Low { get; set; } = null!;

    public string High { get; set; } = null!;

    /// <summary>
    /// Ngày viết
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Ngày đăng
    /// </summary>
    public DateTime? PublicDate { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Property RealEstate { get; set; } = null!;

    public virtual Seller Seller { get; set; } = null!;
}
