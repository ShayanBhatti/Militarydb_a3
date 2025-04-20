using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    public class PersonnelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonnelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Personnel
        public async Task<IActionResult> Index()
        {
            return View(await _context.Personnel
                .Include(p => p.UnitnameNavigation)
                .ToListAsync());
        }

        // GET: Personnel/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Units = await _context.Units
                .Select(u => new SelectListItem { Value = u.Unitname, Text = u.Unitname })
                .ToListAsync();
            return View();
        }

        // POST: Personnel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Rank,Unitname,Contactnumber,Email,Joiningdate,Emergencycontact,Bloodgroup,Weaponassigned,Dutystatus")] Personnel personnel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Ensure UTC date
                    personnel.Joiningdate = personnel.Joiningdate.HasValue 
                        ? DateTime.SpecifyKind(personnel.Joiningdate.Value, DateTimeKind.Utc)
                        : DateTime.UtcNow;

                    // Set default values for non-required fields
                    personnel.Dutystatus = string.IsNullOrEmpty(personnel.Dutystatus) ? "Active" : personnel.Dutystatus;
                    personnel.Bloodgroup ??= "";
                    personnel.Weaponassigned ??= "";
                    personnel.Emergencycontact ??= "";

                    // Important: Do not set Personnelid - let the database generate it
                    _context.Personnel.Add(personnel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                if (innerException.Contains("duplicate key"))
                {
                    // Check if it's an email duplicate
                    if (innerException.Contains("email"))
                    {
                        ModelState.AddModelError("Email", "This email address is already registered.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "A record with this information already exists.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"Database error: {innerException}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
            }

            // If we got this far, something failed
            ViewBag.Units = await _context.Units
                .Select(u => new SelectListItem { Value = u.Unitname, Text = u.Unitname })
                .ToListAsync();

            return View(personnel);
        }

        // GET: Personnel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personnel = await _context.Personnel
                .Include(p => p.UnitnameNavigation)
                .FirstOrDefaultAsync(m => m.Personnelid == id);

            if (personnel == null)
            {
                return NotFound();
            }

            ViewBag.Units = await _context.Units
                .Select(u => new SelectListItem 
                { 
                    Value = u.Unitname, 
                    Text = u.Unitname,
                    Selected = u.Unitname == personnel.Unitname 
                })
                .ToListAsync();

            return View(personnel);
        }

        // POST: Personnel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Personnelid,Name,Rank,Unitname,Contactnumber,Email,Joiningdate,Emergencycontact,Bloodgroup,Weaponassigned,Dutystatus")] Personnel personnel)
        {
            if (id != personnel.Personnelid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure UTC date
                    if (personnel.Joiningdate.HasValue)
                    {
                        personnel.Joiningdate = DateTime.SpecifyKind(personnel.Joiningdate.Value, DateTimeKind.Utc);
                    }

                    _context.Update(personnel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PersonnelExists(personnel.Personnelid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. The record was modified by another user.");
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Unable to save changes. {ex.Message}");
                    // Log the error
                    System.Diagnostics.Debug.WriteLine($"Error saving changes: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An unexpected error occurred. {ex.Message}");
                    // Log the error
                    System.Diagnostics.Debug.WriteLine($"Unexpected error: {ex.Message}");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Units = await _context.Units
                .Select(u => new SelectListItem 
                { 
                    Value = u.Unitname, 
                    Text = u.Unitname,
                    Selected = u.Unitname == personnel.Unitname 
                })
                .ToListAsync();

            return View(personnel);
        }

        // GET: Personnel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personnel = await _context.Personnel
                .Include(p => p.UnitnameNavigation)
                .FirstOrDefaultAsync(m => m.Personnelid == id);

            if (personnel == null)
            {
                return NotFound();
            }

            return View(personnel);
        }

        // GET: Personnel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personnel = await _context.Personnel
                .Include(p => p.UnitnameNavigation)
                .FirstOrDefaultAsync(m => m.Personnelid == id);

            if (personnel == null)
            {
                return NotFound();
            }

            return View(personnel);
        }

        // POST: Personnel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personnel = await _context.Personnel.FindAsync(id);
            if (personnel == null)
            {
                return NotFound();
            }

            try
            {
                _context.Personnel.Remove(personnel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to delete personnel. They may be assigned to missions.");
                return View(personnel);
            }
        }

        private bool PersonnelExists(int id)
        {
            return _context.Personnel.Any(e => e.Personnelid == id);
        }
    }
}