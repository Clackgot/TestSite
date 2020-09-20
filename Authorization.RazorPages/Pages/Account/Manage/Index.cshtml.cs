using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Authorization.RazorPages.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.RazorPages.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        
        [Display(Name = "Email")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public OutputModel Output { get; set; }


        public class InputModel
        {
            [Phone(ErrorMessage = "{0} некорректный")]
            [Display(Name = "Номер телефона")]
            public string PhoneNumber { get; set; }
        }

        public class OutputModel
        {
            [Required(ErrorMessage = "{0} обязательно")]
            [Display(Name = "Имя")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "{0} обязательна")]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "{0} обязательно")]
            [Display(Name = "Отчество")]
            public string MiddleName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
            Output = new OutputModel
            {

                FirstName = user.FirstName,
                MiddleName = user.Middlename,
                LastName = user.LastName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Пользователь с  ID: '{_userManager.GetUserId(User)}' не существует");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Пользователь с  ID: '{_userManager.GetUserId(User)}' не существует");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Некорректный номер телефона";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Ваш профиль был обновлён";
            return RedirectToPage();
        }
    }
}
