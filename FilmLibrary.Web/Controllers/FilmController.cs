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
    public class FilmController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<AppUser> _userManager;
        private FilmLibraryDbContext _dbContext;

        public FilmController(ILogger<HomeController> logger, UserManager<AppUser> userManager, FilmLibraryDbContext dbContext)
        {
            _logger = logger;
            this._userManager = userManager;
            this._dbContext = dbContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var availableFilms = this._dbContext.Films.Include(r => r.Rating).Include(g => g.Genre).Where(f => f.UserName == null).ToList();
            return View(availableFilms);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult IndexAjax(FilmFilterModel filter)
        {
            var filmQuery = this._dbContext.Films.Include(r => r.Rating).Include(g => g.Genre).AsQueryable();
            filter = filter ?? new FilmFilterModel();
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                filmQuery = filmQuery.Where(f => f.Name.ToLower().Contains(filter.Name.ToLower()));
            }
            var films = filmQuery.ToList();
            return PartialView("_IndexTable", films);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            this.FillDropdownRating();
            this.FillDropdownGenre();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Film film)
        {
            this._dbContext.Films.Add(film);
            this._dbContext.SaveChanges();
            this.FillDropdownRating();
            this.FillDropdownGenre();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [Route("film-details/{id:int?}")]
        public IActionResult Details(int id)
        {
            var film = this._dbContext.Films.Include(r => r.Rating).Include(g => g.Genre).Where(f => f.ID == id).FirstOrDefault();
            return View(film);
        }

        [ActionName(nameof(Edit))]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var film = this._dbContext.Films.FirstOrDefault(f => f.ID == id);
            this.FillDropdownRating();
            this.FillDropdownGenre();
            return View(film);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditFilm(int id)
        {
            var film = this._dbContext.Films.FirstOrDefault(f => f.ID == id);
            var canUpdate = await this.TryUpdateModelAsync(film);
            if(canUpdate && this.ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            this.FillDropdownRating();
            this.FillDropdownGenre();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var film = this._dbContext.Films.First(f => f.ID == id);
            this._dbContext.Remove(film);
            this._dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Borrow(int id)
        {
            var UserName = this._userManager.GetUserName(base.User);
            var film = this._dbContext.Films.FirstOrDefault(f => f.ID == id);
            film.UserName = UserName;
            film.BorrowDate = DateTime.Now.ToString();
            var canUpdate = await this.TryUpdateModelAsync(film);
            if (canUpdate && this.ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void FillDropdownRating()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = " - ";
            listItem.Value = "";
            selectItems.Add(listItem);
            foreach(var rate in this._dbContext.Ratings)
            {
                listItem = new SelectListItem(rate.Rate, rate.ID.ToString());
                selectItems.Add(listItem);
            }
            ViewBag.Ratings = selectItems;
        }

        public void FillDropdownGenre()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = " - ";
            listItem.Value = "";
            selectItems.Add(listItem);
            foreach (var genre in this._dbContext.Genres)
            {
                listItem = new SelectListItem(genre.Type, genre.ID.ToString());
                selectItems.Add(listItem);
            }
            ViewBag.Genres = selectItems;
        }
    }
}
