using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesSchool.Models;

namespace RazorPagesSchool.Pages_Departments
{
    public class EditModel : PageModel
    {
        private readonly RazorPagesSchool.Models.SchoolContext _context;

        public EditModel(RazorPagesSchool.Models.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Department = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentId == id);

            if (Department == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var departmentToUpdate = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentId == id);

            if (departmentToUpdate == null)
            {
                return HandleDeletedDepartment();
            }

             _context.Entry(departmentToUpdate)
                .Property("RowVersion").OriginalValue = Department.RowVersion;

            if (await TryUpdateModelAsync<Department>(
                departmentToUpdate,
                "Department",
                s => s.Name, s => s.StartDate, s => s.Budget))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save. " +
                            "The department was deleted by another user.");
                        return Page();
                    }

                    var dbValues = (Department)databaseEntry.ToObject();
                    await setDbErrorMessage(dbValues, clientValues, _context);

                    // Save the current RowVersion so next postback
                    // matches unless an new concurrency issue happens.
                    Department.RowVersion = (byte[])dbValues.RowVersion;
                    // Clear the model error for the next postback.
                    ModelState.Remove("Department.RowVersion");
                }
            }

            return Page();

        }

        private IActionResult HandleDeletedDepartment()
        {
            var deletedDepartment = new Department();
            // ModelState contains the posted data because of the deletion error
            // and will overide the Department instance values when displaying Page().
            ModelState.AddModelError(string.Empty,
                "Unable to save. The department was deleted by another user.");
            return Page();
        }

        private async Task setDbErrorMessage(Department dbValues,
                Department clientValues, SchoolContext context)
        {

            if (dbValues.Name != clientValues.Name)
            {
                ModelState.AddModelError("Department.Name",
                    $"Current value: {dbValues.Name}");
            }
            if (dbValues.Budget != clientValues.Budget)
            {
                ModelState.AddModelError("Department.Budget",
                    $"Current value: {dbValues.Budget:c}");
            }
            if (dbValues.StartDate != clientValues.StartDate)
            {
                ModelState.AddModelError("Department.StartDate",
                    $"Current value: {dbValues.StartDate:d}");
            }

            ModelState.AddModelError(string.Empty,
                "The record you attempted to edit "
              + "was modified by another user after you. The "
              + "edit operation was canceled and the current values in the database "
              + "have been displayed. If you still want to edit this record, click "
              + "the Save button again.");
        }


        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
