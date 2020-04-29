using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using WebApplication10.Controllers;
using Xunit;
using Zero.Models;

namespace NUnitTestProject1
{
    public class HomeContollerTest
    {
        private ApplicationContext db;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _appEnvironment;
        [Fact]
        public void IndexTest()
        {
            // Arrange
            HomeController controller = new HomeController( _logger,db, _appEnvironment);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.Equals("Hello world!", result?.ViewData["Message"]);
            Assert.NotNull(result);
            Assert.Equals("Index", result?.ViewName);
        }
    }
}