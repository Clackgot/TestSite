using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Authorization.RazorPages.Data;
using Authorization.RazorPages.Entities;

namespace Authorization.RazorPages.Pages.Roles
{
    public class DetailsModel : PageModel
    {
        private readonly Authorization.RazorPages.Data.ApplicationDbContext _context;

        public DetailsModel(Authorization.RazorPages.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationRole ApplicationRole { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationRole = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);

            if (ApplicationRole == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
