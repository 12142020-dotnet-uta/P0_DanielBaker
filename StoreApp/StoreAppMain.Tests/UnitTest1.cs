using System;
using Microsoft.EntityFrameworkCore;
using StoreApp;
using Xunit;

namespace StoreAppMain.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            Customer c = new Customer();
            using(var context = new StoreDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                StoreDataLayer repo = new StoreDataLayer(context);

                c = repo.LoginCustomer("test123");
            }

            using(var context = new StoreDbContext(options))
            {
                StoreDataLayer repo = new StoreDataLayer(context);
                Customer result = repo.LoginCustomer("test123");

                Assert.Equal(c.CustomerID, result.CustomerID);
            }


        }
    }
}
