﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RajorTestDemo.Models;

namespace RajorTestDemo.Pages.Employ
{
    public class DeleteModel : PageModel
    {
        private readonly RajorTestDemo.Models.EFCoreDbContext _context;

        public DeleteModel(RajorTestDemo.Models.EFCoreDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RajorTestDemo.Models.Employ Employ { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employ = await _context.Employees.FirstOrDefaultAsync(m => m.Empno == id);

            if (employ == null)
            {
                return NotFound();
            }
            else
            {
                Employ = employ;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employ = await _context.Employees.FindAsync(id);
            if (employ != null)
            {
                Employ = employ;
                _context.Employees.Remove(Employ);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
