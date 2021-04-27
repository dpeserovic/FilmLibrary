using FilmLibrary.DAL;
using FilmLibrary.Model;
using FilmLibrary.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLibrary.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<AppUser> _userManager;
        private FilmLibraryDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, FilmLibraryDbContext dbContext)
        {
            _logger = logger;
            this._userManager = userManager;
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var userName = this._userManager.GetUserName(base.User);
            ViewBag.userName = userName;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
