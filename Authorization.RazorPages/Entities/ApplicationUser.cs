using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.RazorPages.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required(ErrorMessage = "{0} обязательно")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "{0} обязательна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "{0} обязательно")]
        [Display(Name = "Отчество")]
        public string Middlename { get; set; }

        [Phone(ErrorMessage = "{0} некорректный")]
        [Display(Name = "Номер телефона")]
        override public string PhoneNumber { get; set; }
    }
}