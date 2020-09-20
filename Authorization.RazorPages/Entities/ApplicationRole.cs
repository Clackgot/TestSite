using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.RazorPages.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        [Required(ErrorMessage = "{0} обязательно")]
        [Display(Name = "Название")]
        override public string Name { get; set; }

        [Required(ErrorMessage = "{0} обязателен")]
        [Display(Name = "Токен параллелизма")]
        override public string ConcurrencyStamp { get; set; }
    }
}