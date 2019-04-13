using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            return MovieForm();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return MovieForm(movie);
            }

            if (movie.Id == 0)
            {
                _context.Movies.Add(movie);
            }
            else
            {
                var dbMovie = _context.Movies.Single(m => m.Id == movie.Id);
                dbMovie.GenreId = movie.GenreId;
                dbMovie.ReleaseDate = movie.ReleaseDate;
                dbMovie.Name = movie.Name;
                dbMovie.NumberInStock = movie.NumberInStock;
                dbMovie.DateAdded = DateTime.Now;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }

        public ViewResult Index()
        {
            if (User.IsInRole(RoleName.CanManageMovies))
                return View("List");
            else
                return View("ReadOnlyList");
        }

        public ActionResult Details(int id)
        {
            var movies = Movies.SingleOrDefault(m => m.Id == id);

            return View(movies);
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return MovieForm(movie);
        }

        private ActionResult MovieForm()
        {
            var viewModel = new MovieFormViewModel()
            {
                Genres = Genres,
            };

            return View(nameof(MovieForm), viewModel);
        }

        private ActionResult MovieForm(Movie movie)
        {
            var viewModel = new MovieFormViewModel(movie)
            {
                Genres = Genres,
            };

            return View(nameof(MovieForm), viewModel);
        }

        private List<Genre> Genres => _context.Genres.ToList();
        private IQueryable<Movie> Movies => _context.Movies.Include(m => m.Genre);

        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" }
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel);
        }
    }
}