using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModel;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        private VidContext _context;

        public CustomerController()
        {
                _context=new VidContext();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _context.Dispose();
        }

        public ViewResult Index()
        {
            //var customers = _context.Customers.Include(c=>c.MembershipType).ToList();
            //return View(customers);
            return View();
        }

        public ActionResult NewCustomer()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new NewCustomerFormModel
            {
                MembershipTypes = membershipTypes
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCustomerSave(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                
                var viewModel = new NewCustomerFormModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
                return View("NewCustomer", viewModel);
            }
            if(customer.Id==0)
                _context.Customers.Add(customer);
            else
            {
                var customerIndb = _context.Customers.Single(c => c.Id == customer.Id);
                customerIndb.Name = customer.Name;
                customerIndb.MembershipTypeId = customer.MembershipTypeId;
                customerIndb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;
                customerIndb.Birthdate = customer.Birthdate;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Customer");
        }
        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();
            return View(customer);
        }

        public ActionResult EditCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();
            var viewModel = new NewCustomerFormModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            return View("NewCustomer", viewModel);
        }
    }
}
