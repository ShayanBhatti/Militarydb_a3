using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    public class MissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Mission
                .Include(m => m.Personnel)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Personnel = await _context.Personnel
                .Select(p => new SelectListItem { Value = p.Personnelid.ToString(), Text = $"{p.Name} ({p.Rank})" })
                .ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Missioncode,Missiondate")] Mission mission)
        {
            if (ModelState.IsValid)
            {
                mission.ConvertToUtc();
                _context.Add(mission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Personnel = await _context.Personnel
                .Select(p => new SelectListItem { Value = p.Personnelid.ToString(), Text = $"{p.Name} ({p.Rank})" })
                .ToListAsync();
            return View(mission);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mission = await _context.Mission
                .Include(m => m.Personnel)
                .FirstOrDefaultAsync(m => m.Missioncode == id);

            if (mission == null)
            {
                return NotFound();
            }

            // Get all personnel and create SelectListItems
            var allPersonnel = await _context.Personnel.ToListAsync();
            var selectedPersonnelIds = mission.Personnel.Select(p => p.Personnelid).ToList();

            ViewBag.Personnel = allPersonnel.Select(p => new SelectListItem 
            { 
                Value = p.Personnelid.ToString(), 
                Text = $"{p.Name} ({p.Rank})",
                Selected = selectedPersonnelIds.Contains(p.Personnelid)
            }).ToList();

            return View(mission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Missioncode,Missiondate")] Mission mission)
        {
            if (id != mission.Missioncode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mission.ConvertToUtc();
                    _context.Update(mission);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MissionExists(mission.Missioncode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Personnel = await _context.Personnel
                .Select(p => new SelectListItem { Value = p.Personnelid.ToString(), Text = $"{p.Name} ({p.Rank})" })
                .ToListAsync();
            return View(mission);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mission = await _context.Mission
                .Include(m => m.Personnel)
                .FirstOrDefaultAsync(m => m.Missioncode == id);

            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mission = await _context.Mission
                .Include(m => m.Personnel)
                .FirstOrDefaultAsync(m => m.Missioncode == id);

            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var mission = await _context.Mission
                .Include(m => m.Personnel)
                .FirstOrDefaultAsync(m => m.Missioncode == id);

            if (mission != null)
            {
                mission.Personnel.Clear(); // Clear the many-to-many relationships
                _context.Mission.Remove(mission);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MissionExists(string id)
        {
            return _context.Mission.Any(e => e.Missioncode == id);
        }
    }
}