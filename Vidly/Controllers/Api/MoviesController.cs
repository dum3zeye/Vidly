using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<MovieDto> GetMovies()
        {
            return _context.Movies.Include(m => m.Genre).ToList().Select(Mapper.Map<Movie, MovieDto>);
        }

        public IHttpActionResult GetMovie(int id)
        {
            return MovieInDb(id) is Movie movie ? Ok(Mapper.Map<Movie, MovieDto>(movie)) : (IHttpActionResult)NotFound();
        }

        [HttpPost]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var movie = Mapper.Map<MovieDto, Movie>(movieDto);
            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieDto.Id = movie.Id;

            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
        }

        [HttpPut]
        public IHttpActionResult UpdateMovie(int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!(MovieInDb(id) is Movie movie))
            {
                return NotFound();
            }

            Mapper.Map(movieDto, movie);

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteMovie(int id)
        {
            if (!(MovieInDb(id) is Movie movie))
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return Ok();
        }

        private Movie MovieInDb(int id)
        {
            return _context.Movies.SingleOrDefault(m => m.Id == id);
        }
    }
}