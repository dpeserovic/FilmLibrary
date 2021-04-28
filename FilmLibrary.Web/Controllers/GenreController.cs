using FilmLibrary.DAL;
using FilmLibrary.Model;
using FilmLibrary.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLibrary.Web.Controllers
{
    [Authorize]
    public class GenreController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<AppUser> _userManager;
        private FilmLibraryDbContext _dbContext;

        public GenreController(ILogger<HomeController> logger, UserManager<AppUser> userManager, FilmLibraryDbContext dbContext)
        {
            _logger = logger;
            this._userManager = userManager;
            this._dbContext = dbContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var genres = this._dbContext.Genres.AsQueryable().ToList();
            return View(genres);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Genre model)
        {
            this._dbContext.Genres.Add(model);
            this._dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [ActionName(nameof(Edit))]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var genre = this._dbContext.Genres.FirstOrDefault(f => f.ID == id);
            return View(genre);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditGenre(int id)
        {
            var genre = this._dbContext.Genres.FirstOrDefault(f => f.ID == id);
            var canUpdate = await this.TryUpdateModelAsync(genre);
            if (canUpdate && this.ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
