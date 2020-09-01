using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using VetDesk.Entity;
using VetDesk.Repository;

namespace VetDesk.UnitTest
{
    [TestFixture]
    public class CustomerRepoTests : SQLiteTestBase
    {
        private ICustomerRepository repo;

        private int testId;

        [OneTimeSetUp]        
        public void Setup()
        {
            Customer cust = new Customer
            {
                FullName = "Arthur Dent",
                Phone = "222-333-4444",
                Email = "adent@theguide.org"
            };
            context.Customers.Add(cust);
            context.SaveChanges();
            testId = cust.Id;
        }

        [SetUp]
        public void SetupRepo()
        {
            repo = new CustomerRepository(context);
        }

        [Test]
        public void FetchTest()
        {
            Customer c = repo.FetchCustomer(testId);
            Assert.IsNotNull(c);
            Assert.AreEqual(c.Email, "adent@theguide.org");
        }

       
    }
}