using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using AutoMapper;
using Vidly.Models;
using VidlyNew.Dtos;

namespace VidlyNew.Controllers.Api
{
    public class CustomerController : ApiController
    {
        private VidContext _context;

        public CustomerController()
        {
            _context=new VidContext();
        }
        //GET/Api/Customer
        public IHttpActionResult GetCustomers(string query=null)
        {
            var customerQuery = _context.Customers.Include(c => c.MembershipType);
            if (!String.IsNullOrWhiteSpace(query))
            {
                customerQuery = customerQuery.Where(c => c.Name.Contains(query));
            }
            var customerDtos =customerQuery.ToList().Select(Mapper.Map<Customer,CustomerDto>);
            //var customerDtos= _context.Customers.ToList().Select(Mapper.Map<Customer,CustomerDto>);
            return Ok(customerDtos);
        }
        //GET/Api/Customer
        public IHttpActionResult GetCustomer(int Id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == Id);
            if (customer == null)
                return NotFound();
            return Ok(Mapper.Map<Customer,CustomerDto>(customer));
        }
        //POST/Api/Customer
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();
            customerDto.Id = customer.Id;
            return Created(new Uri(Request.RequestUri+"/"+customer.Id),customerDto);
        }
        //PUT/Customer/1
        [HttpPut]
        public IHttpActionResult UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if(customerInDb==null)
                return NotFound();
            Mapper.Map(customerDto, customerInDb);
            _context.SaveChanges();
            return Ok();
        }
        //DELETE/Customer/1
        [HttpDelete]
        public IHttpActionResult DeleteCustomer(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customerInDb == null)
                return NotFound();
            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();
            return Ok();
        }
    }
}
