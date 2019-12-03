using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesSchool.Models;

namespace RazorPagesSchool.Pages_Departments
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesSchool.Models.SchoolContext _context;

        public IndexModel(RazorPagesSchool.Models.SchoolContext context)
        {
            _context = context;
        }

        public IList<Department> Department { get;set; }

        public async Task OnGetAsync()
        {
            Department = await _context.Departments.ToListAsync();
        }
    }
}
