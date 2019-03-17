using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customers
        public ViewResult Index()
        {
            List<Customer> customers = Customers.ToList();

            return View(customers);
        }

        public ActionResult Details(int id)
        {
            var customer = Customers.SingleOrDefault(c => c.Id == id);

            return customer == null ? HttpNotFound() : (ActionResult)View(customer);
        }

        private IQueryable<Customer> Customers => _context.Customers.Include(c => c.MembershipType);
    }
}