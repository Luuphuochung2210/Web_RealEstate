using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_RealEstate.Models;

public partial class LoginUser
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PassWord { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    /// <summary>
    /// 0: inactive 
    /// 1: active
    /// </summary>
    public int Status { get; set; }

    public string? Image { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    [Required(ErrorMessage = "Please choose an Image")]
    [Display(Name = "Upload Image")]
    [NotMapped]
    public IFormFile ImageUpload { get; set; }
}
