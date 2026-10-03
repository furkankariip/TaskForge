using TaskForge.Data;
using Microsoft.AspNetCore.Mvc;
using TaskForge.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TaskForge.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(
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
                .ToList();

            return View(projects);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Project project)
        {
            if (project.DueDate < project.StartDate)
            {
                ModelState.AddModelError(
                    "DueDate",
                    "Bitiş tarihi başlangıç tarihinden önce olamaz."
                );
            }

            if (!ModelState.IsValid)
            {
                return View(project);
            }

            project.CreatedAt = DateTime.Now;
            project.UserId = _userManager.GetUserId(User);

            _context.Projects.Add(project);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            var project = _context.Projects
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        [HttpPost]
        public IActionResult Edit(Project project)
        {
            if (project.DueDate < project.StartDate)
            {
                ModelState.AddModelError(
                    "DueDate",
                    "Bitiş tarihi başlangıç tarihinden önce olamaz."
                );
            }

            if (!ModelState.IsValid)
            {
                return View(project);
            }

            var userId = _userManager.GetUserId(User);

            var existingProject = _context.Projects
                .FirstOrDefault(x => x.Id == project.Id && x.UserId == userId);

            if (existingProject == null)
            {
                return NotFound();
            }

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.StartDate = project.StartDate;
            existingProject.DueDate = project.DueDate;
            existingProject.Status = project.Status;
            existingProject.ProjectPriority = project.ProjectPriority;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var project = _context.Projects
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var project = _context.Projects
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var project = _context.Projects
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
    }
}
