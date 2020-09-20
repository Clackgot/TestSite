using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.RazorPages.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Authorization.RazorPages.Pages.Account.Users
{
    public class RolesModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public List<ApplicationRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }

        public SelectList Staff { get; set; }
        [BindProperty]
        public Guid SelectedStaffId { get; set; }

        public RolesModel(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            AllRoles = new List<ApplicationRole>();
            UserRoles = new List<string>();
        }




        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();


                UserId = user.Id;
                UserEmail = user.Email;
                UserRoles = userRoles;
                AllRoles = allRoles;

                if (AllRoles != null) Staff = new SelectList(AllRoles, nameof(ApplicationRole.Id), nameof(ApplicationRole.Name), null, nameof(ApplicationRole.ConcurrencyStamp));
                return Page();
            }
            return NotFound();
            
        }
    }
}