using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {

        MMABooksContext dbContext;
        Products? p;
        List<Products>? products;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            p = dbContext.Products.Find("A4CS");
            Assert.IsNotNull(p);
            Assert.AreEqual("A4CS", p.ProductCode);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the products that have a unit price of 56.50
            products = dbContext.Products.Where(p => p.UnitPrice == 56.50m).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual(56.50m, products[0].UnitPrice);
            PrintAll(products);
        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }

        [Test]
        public void DeleteTest()
        {
            p = new Products();
            p = dbContext.Products.Find("ZZZZ");
            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find("ZZZZ"));
        }

        [Test]
        public void CreateTest()
        {
            p = new Products();
            p.ProductCode = "ZZZZ";
            p.Description = "Sleepy's code book";
            dbContext.Products.Add(p);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Products.Find("ZZZZ"));
        }

        [Test]
        public void UpdateTest()
        {
            p = dbContext.Products.Find("ZZZZ");
            p.Description = "Sleepy's Vol.2";
            dbContext.Products.Update(p);
            dbContext.SaveChanges();
            p = dbContext.Products.Find("ZZZZ");
            Assert.AreEqual("Sleepy's Vol.2", p.Description);
        }

        public void PrintAll(List<Products> products)
        {
            foreach (Products p in products)
            {
                Console.WriteLine(p);
            }
        }

    }
}