using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_RealEstate.Models;

public partial class Seller
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PassWord { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [Required(ErrorMessage = "Please choose an Image")]
    [Display(Name = "Upload Image")]
    [NotMapped]
    public IFormFile ImageUpload { get; set; }
}
