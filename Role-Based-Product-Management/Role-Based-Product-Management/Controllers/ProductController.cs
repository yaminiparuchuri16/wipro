using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Role_Based_Product_Management.Data;
using Role_Based_Product_Management.Models;
using Role_Based_Product_Management.Models.ViewModels;
using Role_Based_Product_Management.Services;

namespace Role_Based_Product_Management.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IProductProtector _protector;

        public ProductController(ApplicationDbContext db, IProductProtector protector)
        {
            _db = db; _protector = protector;
        }

        // LIST (Admin + Manager)
        public async Task<IActionResult> Index()
        {
            var items = await _db.Products.AsNoTracking().ToListAsync();
            var vms = items.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = _protector.TryUnprotect(p.ProtectedPrice, out var price) ? price : 0m
            }).ToList();

            return View(vms);
        }

        // CREATE (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() => View(new ProductViewModel());

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var entity = new Product { Name = vm.Name, ProtectedPrice = _protector.Protect(vm.Price) };
            _db.Products.Add(entity);
            await _db.SaveChangesAsync();
            TempData["StatusMessage"] = $"Product \"{vm.Name}\" has been successfully created!";
            return RedirectToAction(nameof(Index));
        }

        // EDIT (Admin + Manager)
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.Products.FindAsync(id);
            if (entity == null) return NotFound();

            var vm = new ProductViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = _protector.TryUnprotect(entity.ProtectedPrice, out var price) ? price : 0m
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = await _db.Products.FindAsync(vm.Id);
            if (entity == null) return NotFound();

            entity.Name = vm.Name;
            entity.ProtectedPrice = _protector.Protect(vm.Price);
            await _db.SaveChangesAsync();

            TempData["StatusMessage"] = $"Product \"{vm.Name}\" has been successfully updated!";
            return RedirectToAction(nameof(Index));
        }

        // DELETE (GET) Admin only — Managers will be redirected to /Account/AccessDenied
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Products.FindAsync(id);
            if (entity == null) return NotFound();

            var vm = new ProductViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = _protector.TryUnprotect(entity.ProtectedPrice, out var price) ? price : 0m
            };
            return View("DeleteConfirm", vm);
        }

        // DELETE (POST) Admin only
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _db.Products.FindAsync(id);
            if (entity == null) return NotFound();

            _db.Products.Remove(entity);
            await _db.SaveChangesAsync();

            TempData["StatusMessage"] = $"Product \"{entity.Name}\" has been deleted!";
            return RedirectToAction(nameof(Index));
        }
    }
}
