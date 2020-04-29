using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zero.Models;

namespace WebApplication10.Controllers
{
    public class СheckController : Controller
    {
        private readonly ApplicationContext _context;

        public СheckController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CheakAsync(int? id)
        {
            var cheak = await _context.checKs.FindAsync(id);
            return View(cheak);
        }

        [HttpGet]
        [Authorize(Roles = "supplier,admin")]
        public async Task<IActionResult> ListCheakS()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.Find(userId).Money;
            ViewData["Money"] = user;
            var cheakSup = (_context.checKs.Where(i => i.IdSup == userId)).ToList();
            return View(cheakSup);
        }

        [Authorize(Roles = "admin,buyers")]
        public async Task<IActionResult> ListCheak()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.Find(userId).Money;
            ViewData["Money"] = user;
            var cheakBuy = (_context.checKs.Where(i => i.IdBuyer == userId)).ToList();
            return View(cheakBuy);
        }

        [HttpPost]
        public async Task<IActionResult> Win (int? id)
        {
            var cheaks = await _context.checKs.FindAsync(id);
            cheaks.status = true;
            cheaks.operation = true;
            var buyer  = await _context.Users.FindAsync(cheaks.IdBuyer);
         //   buyer.Money -= cheaks.Sum;
            var sup = await _context.Users.FindAsync(cheaks.IdSup);
            sup.Money += cheaks.Sum;
            cheaks.work = true;
            _context.Users.Update(buyer);
            _context.Users.Update(sup);
            await _context.SaveChangesAsync();
            return RedirectToAction("FadminLC", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Fail(int? id)
        {
            var cheaks = await _context.checKs.FindAsync(id);
            cheaks.status = false;
            cheaks.operation = true;
            cheaks.work = true;
            var buy = await _context.Users.FindAsync(cheaks.IdBuyer);
            buy.Money += cheaks.Sum;
            _context.Users.Update(buy);
            _context.checKs.Update(cheaks);
            await _context.SaveChangesAsync();
            return RedirectToAction("FadminLC", "Home");          
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.checKs.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Buy()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.Find(userId);
            var car1 = (_context.CartItems.Where(i => i.CartId == userId)).ToList();
            var car = car1[0];
            var sum = 0;
            foreach (var item in car1)
            {
                var prods = (_context.products.Where(i => i.Title == car.Name)).ToList();
                if (prods[0].Col < car.Number)
                {
                    Problem("Недостаточно средств");
                    car.Number = prods[0].Col;

                }
            }
            foreach (var item in car1)
            {
                sum += item.CartPrice;
            }
            if(user.Money < sum)
            {
                return RedirectToAction("Addmoney", "Home");
            }
           user.Money -= sum;     
            int j = 0;         
           
            foreach (var item in car1)
            {
                var prod = (_context.products.Where(i => i.Title == car1[j].Name)).ToList();
               
                    prod[0].Col -= car1[0].Number;
                    _context.products.Update(prod[0]);
                    await _context.SaveChangesAsync();
              
                j++;

            }
                var fadmin = _context.Users.Where(i => i.UserName == "fadmin@mail.ru").ToList();

            fadmin[0].Money += sum;
            
            var sup = await _context.Suppliers.FindAsync(car.ProdDict);
            var Cheak = new Сheck { IdBuyer = userId, Sum = sum, IdSup = car.ProdDict, NameBuyer = user.Name, NameSup = sup.Company, status=true };
            await _context.checKs.AddAsync(Cheak);
            _context.Users.Update(user);
            _context.Users.Update(fadmin[0]);
            foreach (var intem in car1)
            {
                _context.CartItems.Remove(intem);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("BuyerLc", "Home");
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var сheck = await _context.checKs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (сheck == null)
            {
                return NotFound();
            }

            return View(сheck);
        }     
    }
}