using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication10.Models;
using Zero.Models;

namespace WebApplication10.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly ApplicationContext _context;

        public CartItemsController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin,buyers")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CartBuy = (_context.CartItems.Where(i => i.CartId == userId)).ToList();
            var user = _context.Users.Find(userId).Money;
            ViewData["Money"] = user;           
            return View(CartBuy);
        }

        [HttpPost]
        [Authorize(Roles = "admin,buyers")]
        public async Task<IActionResult> Index(int id, int amount)
        {
           

            Product prod = await _context.Products.FindAsync(id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartList = _context.CartItems.Where(i => i.CartId == userId).ToList();
            var items = _context.CartItems.SingleOrDefault(c => c.CartId == userId && c.Name == prod.Title);
            var user = _context.Users.Find(userId).Money;
            ViewData["Money"] = user;

            if (items == null)
            {
                items = new CartItems
                {
                    CartId = userId,
                    Name = prod.Title,
                    ProdDict = prod.IdSupplier,
                    CartPrice = prod.Cost * amount,
                    Number = amount
                };
                _context.CartItems.Add(items);
                cartList.Add(items);
                if (cartList[0].ProdDict != prod.IdSupplier)
                {
                    ViewData["errors"] = "Ой-ой, кажется вы уже оформляете корзину у другого поставщика.Пожалуйста, завершите оформление корзины ";
          //          return View("WrongSupp");
                }
            }
            else
            {
                items.CartPrice += prod.Cost * amount;
                items.Number += amount;
            }
            if (items.Number > prod.Col)
            {
                items.Number = prod.Col;
                items.CartPrice = prod.Cost * items.Number;
            }
            await _context.SaveChangesAsync();
            var CartBuy = await _context.CartItems.Where(i => i.CartId == userId).ToListAsync();
            return Redirect("https://localhost:44349/Home/SupliverProduct/" + prod.IdSupplier);        
        }

        [Authorize(Roles = "admin,buyers")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cartItems = await _context.CartItems.FirstOrDefaultAsync(m => m.Name == id);
            if (cartItems == null)
            {
                return NotFound();
            }
            return View(cartItems);
        }

        [HttpPost]
        [Authorize(Roles = "admin,buyers")]
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _context.CartItems.FindAsync(id);

            _context.CartItems.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}