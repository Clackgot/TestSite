using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Authorization.RazorPages.Data;
using Authorization.RazorPages.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.RazorPages
{
    public class Databaseinitializer
    {

        public static void Init(IServiceProvider scopeServiceProvider)
        {
            var userManager = scopeServiceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = scopeServiceProvider.GetService<RoleManager<ApplicationRole>>();

            roleManager.CreateAsync(new ApplicationRole { Name = "Администратор" });
            roleManager.CreateAsync(new ApplicationRole { Name = "Модератор" });
            roleManager.CreateAsync(new ApplicationRole { Name = "Пользователь" });

            var user = new ApplicationUser
            {
                Email = "admin@example.com",
                UserName = "admin@example.com",
                LastName = "Нетреба",
                FirstName = "Анатолий",
                Middlename = "Александрович"
            };

            userManager.CreateAsync(user, "test1234").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(user, "Администратор");


        }

    }
}
