using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class RentalsController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public IHttpActionResult CreateRental(RentalDto rentalDto)
        {
            var customer = Customers.Single(c => c.Id == rentalDto.CustomerId);

            var movies = Movies.Where(m => rentalDto.MovieIds.Contains(m.Id)).ToList();

            foreach (var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                {
                    return BadRequest("Movie is not available");
                }

                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };
                movie.NumberInStock--;
                _context.Rentals.Add(rental);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            SaveChanges();

            return Ok();
        }

        private DbSet<Movie> Movies => _context.Movies;

        private DbSet<Customer> Customers => _context.Customers;

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}