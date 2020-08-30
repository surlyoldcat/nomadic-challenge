using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VetDesk.Entity;
using VetDesk.Models;

namespace VetDesk.Repository
{
    public interface ICritterRepository
    {
        Critter Create(Critter c);
        void Delete(int id);
        bool DoesCritterExist(int id);
        Critter FetchCritter(int id);
        Critter FetchCritterNoTracking(int id);
        IEnumerable<Critter> ListCritters(ListFetchOptions options);
        IEnumerable<Critter> ListCrittersForCustomer(int customerId);
        IEnumerable<CritterType> ListCritterTypes();
        void Update(Critter c);
    }

    public class CritterRepository : ICritterRepository, IDisposable
    {
        private readonly VetDeskContext context;


        public CritterRepository(VetDeskContext db)
        {
            context = db;
        }

        //yes, this really should be in a separate repo, but that
        //can easily be refactored later.
        public IEnumerable<CritterType> ListCritterTypes()
        {
            return context.CritterTypes.OrderBy(ct => ct.Description);
        }

        public Critter FetchCritterNoTracking(int id)
        {
            return context.Critters
                .Include(cr => cr.Customer)
                .Include(cr => cr.CritterType)
                .AsNoTracking()
                .FirstOrDefault(cr => cr.Id == id);
        }

        public Critter FetchCritter(int id)
        {
            return context.Critters
                .Include(cr => cr.Customer)
                .Include(cr => cr.CritterType)
                .FirstOrDefault(cr => cr.Id == id);
        }

        public IEnumerable<Critter> ListCritters(ListFetchOptions options)
        {
            //TODO handle options
            return context.Critters
                .Include(cr => cr.Customer)
                .Include(cr => cr.CritterType)
                .OrderBy(cr => cr.Name);
        }

        public IEnumerable<Critter> ListCrittersForCustomer(int customerId)
        {
            return context.Critters
                .Include(cr => cr.Customer)
                .Include(cr => cr.CritterType)
                .Where(cr => cr.CustomerId == customerId)
                .OrderBy(cr => cr.Name);
        }

        public Critter Create(Critter c)
        {
            context.Add(c);
            context.SaveChanges();
            return c;
        }

        public void Update(Critter c)
        {
            context.Update(c);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cr = new Critter { Id = id };
            context.Remove(cr);
            context.SaveChanges();
        }

        public bool DoesCritterExist(int id)
        {
            return context.Critters.Any(cr => cr.Id == id);
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
