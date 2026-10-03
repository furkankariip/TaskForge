using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskForge.Data;
using TaskForge.Models;

namespace TaskForge.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var projects = _context.Projects
                .Where(x => x.UserId == userId)
                .Include(x => x.TaskItems)
                .ToList();

            var totalProjects = projects.Count;

            var totalTasks = projects
                .SelectMany(x => x.TaskItems)
                .Count();

            var completedTasks = projects
                .SelectMany(x => x.TaskItems)
                .Count(x => x.IsCompleted);

            var pendingTasks = totalTasks - completedTasks;

            ViewBag.TotalProjects = totalProjects;
            ViewBag.TotalTasks = totalTasks;
            ViewBag.CompletedTasks = completedTasks;
            ViewBag.PendingTasks = pendingTasks;

            return View(projects);
        }
    }
}