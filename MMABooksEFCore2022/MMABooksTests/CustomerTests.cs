using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {
        MMABooksContext dbContext;
        Customer? c;
        List<Customer>? customers;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            customers = dbContext.Customers.OrderBy(c => c.CustomerId).ToList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].CustomerId);
            PrintAll(customers);
        }

            [Test]
        public void GetByPrimaryKeyTest()
        {
            c = dbContext.Customers.Find(1);
            Assert.IsNotNull(c);
            Assert.AreEqual("Molunguri, A", c.Name);
            Console.WriteLine(c);

        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the customers who live in OR
           
            /*customers = dbContext.Customers.Where(c => c.State = "OR").SingleOrDefault();
            Assert.AreEqual(5, customers.Count);
            Assert.AreEqual("OR", customers[0].State);
            PrintAll(customers);*/
        }

        [Test]
        public void GetWithInvoicesTest()
        {
           // get the customer whose id is 20 and all of the invoices for that customer
        }

        [Test]
        public void GetWithJoinTest()
        {
            // get a list of objects that include the customer id, name, statecode and statename
            var customers = dbContext.Customers.Join(
               dbContext.States,
               c => c.StateCode,
               s => s.StateCode,
               (c, s) => new { c.CustomerId, c.Name, c.StateCode, s.StateName }).OrderBy(r => r.StateName).ToList();
            Assert.AreEqual(696, customers.Count);
            // I wouldn't normally print here but this lets you see what each object looks like
            foreach (var c in customers)
            {
                Console.WriteLine(c);
            }
        }

        [Test]
         public void DeleteTest()
         {
             c.CustomerId = 700;
             c.Name = "Doe, John";
             c.Address = "999 Main St Unit Z";
             c.City = "Springtucky";
             c.StateCode = "OR";
             c.ZipCode = "97478";
             dbContext.Customers.Add(c);
             dbContext.SaveChanges();
             Assert.IsNull(dbContext.Customers.Find(700));
         }
        
        [Test]
        public void CreateTest()
        {
            c = new Customer();
            c.CustomerId = 700;
            c.Name = "Doe, John";
            c.Address = "999 Main St Unit Z";
            c.City = "Springtucky";
            c.StateCode = "OR";
            c.ZipCode = "97478";
            dbContext.Customers.Add(c);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Customers.Find(700));
        }

        [Test]
        public void UpdateTest()
        {
            c = dbContext.Customers.Find(700);
            c.Name = "Doge, John";
            dbContext.Customers.Update(c);
            dbContext.SaveChanges();
            c = dbContext.Customers.Find(700);
            Assert.AreEqual("Doge, John", c.Name);
        }

        public void PrintAll(List<Customer> customers)
        {
            foreach (Customer c in customers)
            {
                Console.WriteLine(c);
            }
        }
      
    }
}