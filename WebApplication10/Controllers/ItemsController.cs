using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication10.ViewModels;
using Zero.Models;

namespace WebApplication10.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationContext db;
        public ItemsController(ApplicationContext context)
        {
            db = context;
        }

        [Authorize(Roles = "admin,supplier")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var product = await db.products
                .Where(m => m.IdSupplier == id).ToListAsync();
            return View(product);
        }

        [Authorize(Roles = "admin,supplier")]
        [HttpPost]
        public async Task<IActionResult> Index(int id)
        {
            var product = await db.Products.FindAsync(id);

            if (product != null)
            {
                await db.SaveChangesAsync();
            }

            return View();
        }

        [Authorize(Roles = "admin,supplier")]
        public IActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "admin,supplier")]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModel prod)
        {
            byte[] imageData = null;
            if (prod.Image != null)
            {
                using var binaryReader = new BinaryReader(prod.Image.OpenReadStream());
                imageData = binaryReader.ReadBytes((int)prod.Image.Length);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Product product = new Product { startCol = prod.Col, IdSupplier = userId, Title = prod.Title, Description = prod.Description, Cost = prod.Cost, Image = imageData, Col = prod.Col };
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin,supplier")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var product = await db.Products.FindAsync(id);
            
            return View(product);
        }

        [Authorize(Roles = "admin,supplier")]
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, ItemViewModel prod)
        {
            byte[] imageData = null;

            if (prod.Image != null)
            {
                using var binaryReader = new BinaryReader(prod.Image.OpenReadStream());
                imageData = binaryReader.ReadBytes((int)prod.Image.Length);
            }
            product.Image = imageData;
            product.startCol = prod.Col;
            db.Attach(product).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin,supplier")]
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [Authorize(Roles = "admin,supplier")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}