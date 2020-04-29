using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zero.Models;
using Zero.ViewModels;

namespace WebApplication10.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
       private readonly  IHostingEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, IHostingEnvironment appEnvironment)
        {
            db = context;
            _logger = logger;
            _appEnvironment = appEnvironment;
        }

      

        private ApplicationContext db;
        public ActionResult GetNavBar()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = db.Users.Find(userId).Money;
            ViewData["Money"] = user;
          
            return PartialView("_GetNavBar");
        }

        
        [HttpPost]
        public async Task<IActionResult> Uplvl(SupplierViewModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                Supplier supplier = new Supplier { IdSupplier = userId, Company = model.Company, NumberCard = model.NumberCard, Phone = model.Phone ,Cheak=model.Cheak  };
                if (model.Docyment != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(model.Docyment.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Docyment.Length);
                    }
                    // установка массива байтов
                    supplier.Docyment = imageData;

                    db.Suppliers.Add(supplier);
                    await db.SaveChangesAsync();

                    return RedirectToAction("BuyerLC", "Home");

                }
                return View(model);
            }
             
            catch
            {
           
                ModelState.AddModelError("", "Слишком большой файл для загрзки либо не верный формат!, повторите попытку");
                return RedirectToAction("UPLvl", "Home");
            }
          }
        [Authorize(Roles = "admin,buyers")]
        [HttpGet]
        public IActionResult Uplvl()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = db.Users.Find(userId).Money;
            ViewData["Money"] = user;

            return View();
        }
        [Authorize(Roles = "supplier,admin")]
        public IActionResult SupLC()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = db.Users.Find(userId).Money;
            ViewData["Money"] = user;
           
            return View( (db.Suppliers.Where(i => i.IdSupplier == userId)).ToList());
        }
        public IActionResult AdminLC()
        {
            
            return View();
        }
        [Authorize(Roles = "fadmin")]
        public async Task<IActionResult> FAdminLCAsync()
        {

            return View(await db.checKs.ToListAsync());
        }
        [Authorize(Roles = "admin,buyers")]
        public IActionResult AddMoney()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = db.Users.Find(userId).Money;
            ViewData["Money"] = user;

            return View();
        }
        [Authorize(Roles = "admin,buyers")]
        [HttpPost]
        public async Task<IActionResult> AddMoney(AddMoneyViewModel addMoney)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await db.Users.FindAsync(userId);
            ViewData["Money"] = user;
            user.Money += addMoney.Money;
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("BuyerLC", "home");
        }

            public IActionResult Index()
        {
           
            return View(db.Users.ToList());
        }
        [Authorize(Roles = "admin,buyers")]
        public IActionResult BuyerLC()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user =  db.Users.Find(userId).Money;
            ViewData["Money"] = user;
            return View(db.Users.ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Otch()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = db.Users.Find(userId).Money;
            ViewData["Money"] = user;

            if (userId == null)
            {
                return NotFound();
            }

            var product = db.products
                .Where(m => m.IdSupplier == userId).ToList();
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                db.Files.Add(file);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin,buyers")]
        public IActionResult Supliver()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = db.Users.Find(userId).Money;
            ViewData["Money"] = user;

            return View(db.Suppliers.ToList());
        }
        [Authorize(Roles = "admin,buyers")]
        public async Task<IActionResult> SupliverProduct(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = db.Users.Find(userId).Money;
            ViewData["Money"] = user;

            if (id == null)
            {
                return NotFound();
            }

            var product =  db.products
                .Where(m => m.IdSupplier == id).ToList();
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public ActionResult Export()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var product = db.products
                .Where(m => m.IdSupplier == userId).ToList();
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Отчёт");

                worksheet.Cell("A1").Value = "Id товара";
                worksheet.Cell("B1").Value = "Наименование товара";
                worksheet.Cell("C1").Value = "Кол-во проданного товара";
                worksheet.Cell("D1").Value = "Сумма";
                worksheet.Row(1).Style.Font.Bold = true;
                int i = 0;
                foreach (var item in product) {
                    
                  
                    worksheet.Cell(i + 2, 1).Value = item.IdProd;
                    
                    worksheet.Cell(i + 2, 2).Value = item.Title;
                    worksheet.Cell(i + 2, 3).Value = item.Colprod();
                    worksheet.Cell(i + 2, 4).Value = item.Itogo();

                    i++;
                  
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Отчет_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }        
        }

        [Authorize(Roles = "admin,buyers")]
        public async Task<IActionResult> Details(string id)
        {
            var sup = await db.Suppliers.Where(i => i.IdSupplier == id).ToListAsync();
            return View(sup);
        }

    }

}
