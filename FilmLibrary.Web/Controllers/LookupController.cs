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
    public class LookupController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private FilmLibraryDbContext _dbContext;

        public LookupController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, FilmLibraryDbContext dbContext)
        {
            _logger = logger;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._dbContext = dbContext;
        }
        public IActionResult Index()
        {
            if(this._signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
            {
                var takenFilms = this._dbContext.Films.Include(r => r.Rating).Include(g => g.Genre).Where(f => f.UserName != null).ToList();
                return View(takenFilms);
            }
            else
            {
                var UserName = this._userManager.GetUserName(base.User);
                var myFilms = this._dbContext.Films.Include(r => r.Rating).Include(g => g.Genre).Where(f => f.UserName == UserName).ToList();
                return View(myFilms);
            }
        }
        [Route("lookup-film-details/{id:int?}")]
        public IActionResult Details(int id)
        {
            var film = this._dbContext.Films.Include(r => r.Rating).Include(g => g.Genre).Where(f => f.ID == id).FirstOrDefault();
            return View(film);
        }
        public async Task<IActionResult> Return(int id)
        {
            var film = this._dbContext.Films.FirstOrDefault(f => f.ID == id);
            film.UserName = null;
            film.BorrowDate = null;
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
    }
}
