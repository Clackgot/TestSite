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

            roleManager.CreateAsync(new ApplicationRole { Name = "Administrator" });
            roleManager.CreateAsync(new ApplicationRole { Name = "Manager" });
            roleManager.CreateAsync(new ApplicationRole { Name = "User" });



            var user = new ApplicationUser
            { 
                // = new Guid("344aea22-2f13-4de8-ab87-e694d81df74a"),
                Email = "admin@example.com",
                UserName = "admin@example.com",
                LastName = "Нетреба",
                FirstName = "Анатолий",
                Middlename = "Александрович"
            };

            var resultCreateUser = userManager.CreateAsync(user, "1488Bambuk)").GetAwaiter().GetResult();

            //if(result.Succeeded)
            //{
            //    userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Administrator")).GetAwaiter().GetResult();

            //}


            var roleAdministrator = roleManager.FindByNameAsync("Administrator").GetAwaiter().GetResult();
            if (roleAdministrator == null)
            {
                var createRoleResult = roleManager.CreateAsync(new ApplicationRole { Name = "Administrator" });


                //Add default User to Role Admin    
                if (createRoleResult.GetAwaiter().GetResult().Succeeded)
                {
                    var resultAddedRole = userManager.AddToRoleAsync(user, "Administrator");
                    return;
                }
            }
            var result3 = userManager.AddToRoleAsync(user, "Administrator");
        }

    }
}
