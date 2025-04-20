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
            ViewBag.Units = await GetUnitsListAsync();
            return View();
        }

        // POST: Personnel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Personnelid,Name,Rank,Unitname,Contactnumber,Email,Joiningdate,Emergencycontact,Bloodgroup,Weaponassigned,Dutystatus")] Personnel personnel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if ID already exists
                    if (await _context.Personnel.AnyAsync(p => p.Personnelid == personnel.Personnelid))
                    {
                        ModelState.AddModelError("Personnelid", "This Personnel ID is already in use.");
                        ViewBag.Units = await GetUnitsListAsync();
                        return View(personnel);
                    }

                    // Set default values and UTC date
                    personnel.Joiningdate = personnel.Joiningdate.HasValue 
                        ? DateTime.SpecifyKind(personnel.Joiningdate.Value, DateTimeKind.Utc)
                        : DateTime.UtcNow;
                    
                    personnel.Dutystatus ??= "Active";
                    personnel.Bloodgroup ??= "";
                    personnel.Weaponassigned ??= "";
                    personnel.Emergencycontact ??= "";

                    _context.Personnel.Add(personnel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            ViewBag.Units = await GetUnitsListAsync();
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

        [HttpGet]
        public async Task<JsonResult> CheckIdExists(int id)
        {
            var exists = await _context.Personnel.AnyAsync(p => p.Personnelid == id);
            return Json(exists);
        }

        private async Task<List<SelectListItem>> GetUnitsListAsync()
        {
            return await _context.Units
                .Select(u => new SelectListItem { Value = u.Unitname, Text = u.Unitname })
                .ToListAsync();
        }

        private bool PersonnelExists(int id)
        {
            return _context.Personnel.Any(e => e.Personnelid == id);
        }
    }
}