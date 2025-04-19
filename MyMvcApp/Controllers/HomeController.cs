using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Models;
using MyMvcApp.Data;

namespace MyMvcApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            TotalPersonnel = await _context.Personnel.CountAsync(),
            TotalUnits = await _context.Units.CountAsync(),
            TotalMissions = await _context.Mission.CountAsync(),
            RecentMissions = await _context.Mission
                .OrderByDescending(m => m.Missiondate)
                .Take(5)
                .ToListAsync()
        };

        return View(viewModel);
    }
}
