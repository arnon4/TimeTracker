using MegoTimeTracker.Models;
using MegoTimeTracker.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MegoTimeTracker.Controllers {
    public class TimeController : Controller {
        private readonly IDbRepository _repo;
        public TimeController(IDbRepository repo) => _repo = repo;
        public async Task<IActionResult> ToggleAsync(int id, string time) {
            try {
                var isCorrectId = await _repo.LogState(id, time);
                if (!isCorrectId) {
                    var error = "ID doesn't exist in database. If it was entered correctly, contact an admin to add you.";
                    return RedirectToAction("Index", "Home", new { error });
                }

                return View();
            } catch (Exception) {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}