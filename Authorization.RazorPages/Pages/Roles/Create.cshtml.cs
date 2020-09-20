using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Authorization.RazorPages.Data;
using Authorization.RazorPages.Entities;

namespace Authorization.RazorPages.Pages.Roles
{
    public class CreateModel : PageModel
    {
        private readonly Authorization.RazorPages.Data.ApplicationDbContext _context;

        public CreateModel(Authorization.RazorPages.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ApplicationRole ApplicationRole { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Roles.Add(ApplicationRole);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
