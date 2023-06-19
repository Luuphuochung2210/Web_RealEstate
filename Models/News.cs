using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_RealEstate.Models;

public partial class News
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Contents { get; set; } = null!;

    public string? Title { get; set; }

    public string? Image { get; set; }

    [Required(ErrorMessage = "Please choose an Image")]
    [Display(Name = "Upload Image")]
    [NotMapped]
    public IFormFile ImageUpload { get; set; }
}
