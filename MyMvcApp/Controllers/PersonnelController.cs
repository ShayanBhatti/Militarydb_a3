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
        public async Task<IActionResult> Create([Bind("Name,Rank,Unitname,ContactNumber,Email,JoiningDate,EmergencyContact,BloodGroup,WeaponAssigned,DutyStatus")] Personnel personnel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personnel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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

            var personnel = await _context.Personnel.FindAsync(id);
            if (personnel == null)
            {
                return NotFound();
            }

            ViewBag.Units = await _context.Units
                .Select(u => new SelectListItem { Value = u.Unitname, Text = u.Unitname })
                .ToListAsync();
            return View(personnel);
        }

        // POST: Personnel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonnelID,Name,Rank,Unitname,ContactNumber,Email,JoiningDate,EmergencyContact,BloodGroup,WeaponAssigned,DutyStatus")] Personnel personnel)
        {
            if (id != personnel.Personnelid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personnel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonnelExists(personnel.Personnelid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.Units = await _context.Units
                .Select(u => new SelectListItem { Value = u.Unitname, Text = u.Unitname })
                .ToListAsync();
            return View(personnel);
        }

        private bool PersonnelExists(int id)
        {
            return _context.Personnel.Any(e => e.Personnelid == id);
        }
    }
}