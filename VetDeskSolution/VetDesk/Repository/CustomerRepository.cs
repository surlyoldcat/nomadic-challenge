using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VetDesk.Entity;

namespace VetDesk.Repository
{
    public interface ICustomerRepository
    {
        Customer FetchCustomer(int id);
        Customer CreateCustomer(Customer c);
        void UpdateCustomer(Customer c);
        void DeleteCustomer(int id);
        bool DoesCustomerExist(int id);
        IQueryable<Customer> CustomersQueryable();
    }

    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private readonly VetDeskContext context;
        
        public CustomerRepository(VetDeskContext db)
        {
            context = db;
        }

        public Customer CreateCustomer(Customer c)
        {
            context.Customers.Add(c);
            context.SaveChanges();
            return c;

        }

        public void DeleteCustomer(int id)
        {
            var cust = new Customer { Id = id };
            context.Customers.Remove(cust);
            context.SaveChanges();
        }

        public IQueryable<Customer> CustomersQueryable()
        {
            return context.Customers
                .Include(c => c.Critters)
                .AsNoTracking();
        }

        

        public Customer FetchCustomer(int id)
        {
            return context.Customers
                .Include(cu => cu.Critters)
                .FirstOrDefault(c => c.Id == id);
        }

        public void UpdateCustomer(Customer c)
        {
            context.Customers.Update(c);
            context.SaveChanges();
        }

        public bool DoesCustomerExist(int id)
        {
            return context.Customers.Any(c => c.Id == id);
        }

        #region Disposable pattern
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
