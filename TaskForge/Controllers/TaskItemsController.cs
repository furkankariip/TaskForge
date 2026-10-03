using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskForge.Data;
using TaskForge.Models;

namespace TaskForge.Controllers
{
    [Authorize]
    public class TaskItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskItemsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int projectId)
        {
            var userId = _userManager.GetUserId(User);

            var project = _context.Projects
                .FirstOrDefault(x => x.Id == projectId && x.UserId == userId);

            if (project == null)
            {
                return NotFound();
            }

            var taskItems = _context.TaskItems
                .Where(x => x.ProjectId == projectId)
                .ToList();

            ViewBag.ProjectId = project.Id;
            ViewBag.ProjectName = project.Name;

            return View(taskItems);
        }

        [HttpGet]
        public IActionResult Create(int projectId)
        {
            var userId = _userManager.GetUserId(User);

            var project = _context.Projects
                .FirstOrDefault(x => x.Id == projectId && x.UserId == userId);

            if (project == null)
            {
                return NotFound();
            }

            var taskItem = new TaskItem
            {
                ProjectId = projectId
            };

            ViewBag.ProjectName = project.Name;

            return View(taskItem);
        }
        [HttpPost]
        public IActionResult Create(TaskItem taskItem)
        {
            if (!ModelState.IsValid)
            {
                return View(taskItem);
            }

            var userId = _userManager.GetUserId(User);

            var project = _context.Projects
                .FirstOrDefault(x =>
                    x.Id == taskItem.ProjectId &&
                    x.UserId == userId);

            if (project == null)
            {
                return NotFound();
            }

            taskItem.CreatedAt = DateTime.Now;
            taskItem.IsCompleted = false;

            _context.TaskItems.Add(taskItem);
            _context.SaveChanges();

            return RedirectToAction("Index",
                new { projectId = taskItem.ProjectId });
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            var taskItem = _context.TaskItems
                .FirstOrDefault(x =>
                    x.Id == id &&
                    x.Project!.UserId == userId);

            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }
        [HttpPost]
        public IActionResult Edit(TaskItem taskItem)
        {
            if (!ModelState.IsValid)
            {
                return View(taskItem);
            }

            var userId = _userManager.GetUserId(User);

            var existingTask = _context.TaskItems
                .FirstOrDefault(x =>
                    x.Id == taskItem.Id &&
                    x.Project!.UserId == userId);

            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = taskItem.Title;
            existingTask.Description = taskItem.Description;
            existingTask.DueDate = taskItem.DueDate;
            existingTask.IsCompleted = taskItem.IsCompleted;

            _context.SaveChanges();

            return RedirectToAction(
                "Index",
                new { projectId = existingTask.ProjectId }
            );
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var taskItem = _context.TaskItems
                .FirstOrDefault(x =>
                    x.Id == id &&
                    x.Project!.UserId == userId);

            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var taskItem = _context.TaskItems
                .FirstOrDefault(x =>
                    x.Id == id &&
                    x.Project!.UserId == userId);

            if (taskItem == null)
            {
                return NotFound();
            }

            var projectId = taskItem.ProjectId;

            _context.TaskItems.Remove(taskItem);
            _context.SaveChanges();

            return RedirectToAction(
                "Index",
                new { projectId = projectId }
            );
        }
    }
}