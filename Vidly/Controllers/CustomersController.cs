using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

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

        public ActionResult CustomerForm()
        {
            return RedirectToAction(nameof(New));
        }

        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes,
            };

            return View(nameof(CustomerForm), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return CustomerForm(customer);
            }

            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);

                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribed = customer.IsSubscribed;
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index), "Customers");
        }

        // GET: Customers
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var customer = Customers.SingleOrDefault(c => c.Id == id);

            return customer == null ? HttpNotFound() : (ActionResult)View(customer);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return CustomerForm(customer);
        }

        private ActionResult CustomerForm(Customer customer)
        {
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            return View(nameof(CustomerForm), viewModel);
        }

        private IQueryable<Customer> Customers => _context.Customers.Include(c => c.MembershipType);
    }
}