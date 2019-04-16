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

        public IEnumerable<RentalDto> GetRentals()
        {
            return Rentals.Include(c => c.Movie).Include(c => c.Id).Select(Mapper.Map<Rental, RentalDto>);
        }

        public IHttpActionResult GetRental(int id)
        {
            return RentalInDatabase(id) is Rental rental ? Ok(Mapper.Map<Rental, RentalDto>(rental)) : (IHttpActionResult)NotFound();
        }

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

        [HttpPut]
        public IHttpActionResult UpdateRental(int id, RentalDto rentalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!(RentalInDatabase(id) is Rental rental))
            {
                return NotFound();
            }

            Mapper.Map(rentalDto, rental);

            SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteRental(int id)
        {
            if (!(RentalInDatabase(id) is Rental rental))
            {
                return NotFound();
            }

            Rentals.Remove(rental);
            SaveChanges();

            return Ok();
        }

        private DbSet<Rental> Rentals => _context.Rentals;
        private DbSet<Movie> Movies => _context.Movies;
        private DbSet<Customer> Customers => _context.Customers;

        private void SaveChanges()
        {
            _context.SaveChanges();
        }

        private Rental RentalInDatabase(int id)
        {
            return Rentals.SingleOrDefault(r => r.Id == id);
        }
    }
}