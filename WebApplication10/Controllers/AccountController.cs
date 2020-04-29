using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zero.Models;
using Zero.ViewModels;

namespace WebApplication10.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;
       RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext contex)
        {
            db = contex;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Buyer(BuyerViewModel model)
        {
            if (ModelState.IsValid)
            {
               // var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                string ID = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
                Buyer buyer = new Buyer { IdBuyer= ID, Name = User.Identity.Name , Adrees = model.Adrees,Card = model.Card };
                db.buyers.Add(buyer);
                await db.SaveChangesAsync();
                return RedirectToAction("index", "Home");

            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Buyer()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            //if (User.IsInRole("admin"))
            //{
            //    return RedirectToAction("AdminLC", "Home");
            //}
            //if (User.IsInRole("fadmin"))
            //{
            //    return RedirectToAction("FadminLC", "Home");
            //}
            //if (User.IsInRole("supplier"))
            //{
            //    return RedirectToAction("SupLC", "Home");
            //}
            //if (User.IsInRole("buyers"))
            //{
            //    return RedirectToAction("BuyerLC", "Home");
            //}

            return View();
        }
        [HttpPost]
       
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName =model.Email, Name = model.Name, Patronymic= model.Patronymic, Famil = model.Famil };
                // добавляем пользователя

                var result = await _userManager.CreateAsync(user, model.Password);
              string ID = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
                var buyer = new Buyer() { IdBuyer = ID };
                db.buyers.Add(buyer);
                await db.SaveChangesAsync();
                if (result.Succeeded)
                {
                    // установка куки
                    await _userManager.AddToRoleAsync(user, "buyers");
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("BuyerLC", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            //if (User.IsInRole("admin"))
            //{
            //    return RedirectToAction("AdminLC", "Home");
            //}
            //if (User.IsInRole("fadmin"))
            //{
            //    return RedirectToAction("FadminLC", "Home");
            //}
            //if (User.IsInRole("supplier"))
            //{
            //    return RedirectToAction("SupLC", "Home");
            //}
            //if (User.IsInRole("buyers"))
            //{
            //    return RedirectToAction("BuyerLC", "Home");
            //}


            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {

                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {

                        var user = await _userManager.FindByEmailAsync(model.Email);
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles[0] == "admin")
                            return RedirectToAction("AdminLC", "Home");
                        if (roles[0] == "fadmin")
                            return RedirectToAction("FadminLC", "Home");
                        if (roles[0] == "buyers")
                            return RedirectToAction("BuyerLC", "Home");
                        if (roles[0] == "supplier")
                            return RedirectToAction("SupLC", "Home");
                        return RedirectToAction("index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult SupUp()
        {
            return View(db.Suppliers.ToList());
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> SupUp(string id)
        {          
            var supplier = await db.Suppliers.FindAsync(id);                
            User user = await _userManager.FindByIdAsync(id);
            supplier.Cheak = true;
            db.Suppliers.Update(supplier);
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, "supplier");
           
            await db.SaveChangesAsync();
            return RedirectToAction("SupUp", "Account");
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DelSup(string id)
        {
            Supplier supplier = await db.Suppliers.FindAsync(id);
            db.Suppliers.Remove(supplier);
            await db.SaveChangesAsync();
            return RedirectToAction("SupUP", "Account");
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}