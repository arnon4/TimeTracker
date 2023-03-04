using MegoTimeTracker.Models;
using MegoTimeTracker.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MegoTimeTracker.Controllers {
    public class AuthController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IDbRepository _repo;
        public AuthController(SignInManager<IdentityUser> manager, UserManager<IdentityUser> userManager, IDbRepository repo) {
            _signInManager = manager;
            _userManager = userManager;
            _repo = repo;
        }
        public IActionResult Login(string? error) {
            return View("Login", error);
        }
        public async Task<IActionResult> Authenticate(string name, string password) {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null) {
                return View("Login", "Incorrect username");
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded) {
                return View("Login", "Incorrect password");
            }

            var students = await _repo.GetStudentsAsync();
            var ids = students.Select(x => x.IdNumber);
            return View("Admin", ids);
        }

        [Authorize]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Admin() {
            var students = await _repo.GetStudentsAsync();
            var ids = students.Select(x => x.IdNumber);
            return View(ids);
        }
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldpass, string newpass) {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
            await _userManager.ChangePasswordAsync(user!, oldpass, newpass);

            var students = await _repo.GetStudentsAsync();
            var ids = students.Select(x => x.IdNumber);
            return View("Admin", ids);
        }
    }
}
