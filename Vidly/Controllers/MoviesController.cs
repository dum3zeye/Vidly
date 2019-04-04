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

        public ActionResult New()
        {
            var genres = _context.Genres.ToList();

            var viewModel = new MovieFormViewModel
            {
                Genres = genres,
                Command = "New Movie"
            };

            return View("MovieForm", viewModel);
        }

        public ActionResult Save(Movie movie)
        {
            movie.DateAdded = DateTime.Now;
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
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }

        public ViewResult Index()
        {
            var movies = Movies.ToList();

            return View(movies);
        }

        public ActionResult Details(int id)
        {
            var movies = Movies.SingleOrDefault(m => m.Id == id);

            return View(movies);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
                Genres = _context.Genres.ToList(),
                Command = "Edit Movie"
            };

            return View("MovieForm", viewModel);
        }

        private IQueryable<Movie> Movies => _context.Movies.Include(m => m.Genre);

        // GET: Movies/Random
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