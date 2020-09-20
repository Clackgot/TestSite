using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Authorization.RazorPages.Pages
{
    [AllowAnonymous]
    public class TaskModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
