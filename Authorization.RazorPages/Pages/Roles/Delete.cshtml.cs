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
    public class DeleteModel : PageModel
    {
        private readonly Authorization.RazorPages.Data.ApplicationDbContext _context;

        public DeleteModel(Authorization.RazorPages.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationRole = await _context.Roles.FindAsync(id);

            if (ApplicationRole != null)
            {
                _context.Roles.Remove(ApplicationRole);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
