using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Role_Based_Product_Management.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Role_Based_Product_Management.Controllers
{
    public class RegisterViewModel
    {
        [Required] public string Username { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
        [Required] public string Role { get; set; } = ""; // Admin or Manager
    }

    public class LoginViewModel
    {
        [Required] public string Username { get; set; } = "";
        [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
        public bool RememberMe { get; set; }
    }

    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager; _userManager = userManager;
        }

        //[HttpGet] public IActionResult Register() => View();
        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //public IActionResult Register() => View();

        //[Authorize(Roles = "Admin")]
        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel vm)
        //{
        //    if (!ModelState.IsValid) return View(vm);
        //    if (vm.Role != "Admin" && vm.Role != "Manager")
        //    {
        //        ModelState.AddModelError("", "Invalid role."); return View(vm);
        //    }

        //    var user = new ApplicationUser { UserName = vm.Username, Email = vm.Email, EmailConfirmed = true };
        //    var result = await _userManager.CreateAsync(user, vm.Password);
        //    if (!result.Succeeded)
        //    {
        //        foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
        //        return View(vm);
        //    }

        //    await _userManager.AddToRoleAsync(user, vm.Role);
        //    TempData["StatusMessage"] = $"User '{vm.Username}' registered as {vm.Role}.";
        //    return RedirectToAction("Login");
        //}
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register() => View();

        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Decide which role to assign safely
            var roleToAssign = "Manager"; // default for anonymous/self-registration

            if (User.Identity?.IsAuthenticated == true)
            {
                // Only a signed-in Admin may choose the role
                var currentUser = await _userManager.GetUserAsync(User);
                if (await _userManager.IsInRoleAsync(currentUser!, "Admin"))
                    roleToAssign = (vm.Role == "Admin" || vm.Role == "Manager") ? vm.Role : "Manager";
            }

            // Bootstrap: if no users exist yet, allow first registration to be Admin
            if (!_userManager.Users.Any() && vm.Role == "Admin")
                roleToAssign = "Admin";

            var user = new ApplicationUser { UserName = vm.Username, Email = vm.Email, EmailConfirmed = true };
            var create = await _userManager.CreateAsync(user, vm.Password);
            if (!create.Succeeded)
            {
                foreach (var e in create.Errors) ModelState.AddModelError("", e.Description);
                return View(vm);
            }

            await _userManager.AddToRoleAsync(user, roleToAssign);
            TempData["StatusMessage"] = $"User '{vm.Username}' registered as {roleToAssign}.";
            return RedirectToAction("Login");
        }




        [HttpGet] public IActionResult Login(string? returnUrl = null) { ViewBag.ReturnUrl = returnUrl; return View(); }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);
            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction("Index", "Product");
            }
            ModelState.AddModelError("", "Invalid credentials.");
            return View(vm);
        }

        [HttpPost] public async Task<IActionResult> Logout() { await _signInManager.SignOutAsync(); return RedirectToAction("Login"); }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            //ViewBag.Message = "Access Denied: You do not have permission to perform this product.";
            //return View();

            // Single, canonical message for all access-denied cases
            ViewData["AccessDeniedMessage"] = "Access Denied: You do not have permission to delete products.";
            return View();
        }
    }
}
