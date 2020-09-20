﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Authorization.RazorPages.Data;
using Authorization.RazorPages.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Authorization.RazorPages.Pages.Roles
{
    [Authorize(Policy = "Администратор")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ApplicationRole> ApplicationRole { get;set; }

        public async Task OnGetAsync()
        {
            ApplicationRole = await _context.Roles.ToListAsync();
        }
    }
}
