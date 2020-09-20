using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Authorization.RazorPages.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Authorization.RazorPages.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; } //URL с которым пришёл пользователь

        public class InputModel
        {
            [Required(ErrorMessage = "{0} обязательно")]
            [Display(Name = "Имя")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "{0} обязательна")]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Display(Name = "Отчество")]
            public string Middlename { get; set; }

            [Required(ErrorMessage = "{0} обязателен")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} обязателен")]
            [StringLength(100, ErrorMessage = "{0} должен иметь минимум {2} символов и {1} максимум", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Required(ErrorMessage = "{0} обязательно")]
            [DataType(DataType.Password)]
            [Display(Name = "Подтверждение пароля")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            public string ConfirmPassword { get; set; }
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)//Если модель валидна
            {
                var user = new ApplicationUser
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Middlename = Input.Middlename,
                    Email = Input.Email,
                    UserName = Input.Email
                };//Перенос данных из представления в модель
                var result = await _userManager.CreateAsync(user, Input.Password); //Создание пользователя, с заданным паролем
                if (result.Succeeded)//Если пользователь создан
                {
                    _logger.LogInformation($"Пользователь {user.LastName} {user.FirstName} успешно создан");//Вывод информации в логгер

                    try
                    {
                        var addedRoleToUser = await _userManager.AddToRoleAsync(user, "Пользователь");
                    }
                    catch
                    {
                        return NotFound("Не удалось присвоить пользователю роль \"Пользователь\"");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);//Вход в аккаунт созданного пользователя
                    return LocalRedirect(returnUrl);//Переход к запрашиваему ресурсу
                }
                foreach (var error in result.Errors)//Сохранение ошибок в модель
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
