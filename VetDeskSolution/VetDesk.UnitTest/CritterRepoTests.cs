using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using VetDesk.Entity;
using VetDesk.Repository;

namespace VetDesk.UnitTest
{
    [TestFixture]
    public class CritterRepoTests : SQLiteTestBase
    {
        private ICritterRepository repo;

        private int testCustomerId;
        private int testCritterId;

        [OneTimeSetUp]
        public void Setup()
        {
            Customer cust = new Customer
            {
                FullName = "Joe Blow",
                Phone = "987-654-3210",
                Email = "joe@fakedomain.com"
            };
            context.Customers.Add(cust);
            context.SaveChanges();
            testCustomerId = cust.Id;

            Critter crit = new Critter
            {
                Name = "Rex",
                CritterTypeId = 1,
                Color = "Blue",
                LastWeight = 600,
                CustomerId = testCustomerId,
                PhotoId = 1
            };
            context.Critters.Add(crit);
            context.SaveChanges();
            testCritterId = crit.Id;

        }

        [SetUp]
        public void SetupRepo()
        {
            repo = new CritterRepository(context);
        }

        [Test]
        public void FetchTest()
        {
            Critter crit = repo.FetchCritter(testCritterId);
            Assert.IsNotNull(crit);
            Assert.AreEqual("Rex", crit.Name);
            Assert.AreEqual("Joe Blow", crit.Customer.FullName);
        }
    }
}