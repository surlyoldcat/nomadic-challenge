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
        IEnumerable<CritterType> ListCritterTypes();
        void Update(Critter c);
        IQueryable<Critter> CrittersQueryable();
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

        public IQueryable<Critter> CrittersQueryable()
        {
            return context.Critters
                .Include(cr => cr.Customer)
                .Include(cr => cr.CritterType)
                .AsNoTracking();

        }

       
        public Critter FetchCritter(int id)
        {
            return context.Critters
                .Include(cr => cr.Customer)
                .Include(cr => cr.CritterType)
                .FirstOrDefault(cr => cr.Id == id);
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
