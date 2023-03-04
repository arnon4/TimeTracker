using MegoTimeTracker.Models;
using MegoTimeTracker.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MegoTimeTracker.Controllers {
    [Authorize]
    public class ManagementController : Controller {
        private readonly IDbRepository _repo;
        public ManagementController(IDbRepository repo) => _repo = repo;
        public async Task<IActionResult> ViewAttendances(string? id) {
            if (id == null) {
                return View(default(Student));
            }

            if (!int.TryParse(id, out int userId)) {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var user = await _repo.GetStudentAsync(userId);
            return View(user);
        }
        public async Task<IActionResult> AddStudent(int idNumber) {
            await _repo.AddStudentAsync(idNumber);
            var students = await _repo.GetStudentsAsync();
            var ids = students.Select(x => x.IdNumber);
            return RedirectToAction("Admin", "Auth", ids);
        }
        public async Task<IActionResult> DeleteStudent(string? id) {
            if (!int.TryParse(id, out int userId)) {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            await _repo.DeleteStudentAsync(userId);
            var students = await _repo.GetStudentsAsync();
            var ids = students.Select(x => x.IdNumber);
            return RedirectToAction("Admin", "Auth", ids);
        }
    }
}
