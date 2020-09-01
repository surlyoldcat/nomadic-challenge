using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using VetDesk.Entity;

namespace VetDesk.Repository
{
    public interface IPhotoRepository
    {
        Photo Create(Photo p);
        Photo Fetch(int id);
        void Delete(int id);
        Photo Update(Photo p);
        bool DoesPhotoExist(int id);
    }

    public class PhotoRepository : IDisposable, IPhotoRepository
    {
        private const int CACHE_EXP_SECONDS = 30;
        
        private readonly VetDeskContext context;
        private readonly IMemoryCache cache;

        public PhotoRepository(VetDeskContext ctx, IMemoryCache memcache)
        {
            context = ctx;
            cache = memcache;
        }

        public Photo Fetch(int id)
        {
            var ph = cache.GetOrCreate<Photo>(id, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(CACHE_EXP_SECONDS);
                return context.Photos.FirstOrDefault(p => p.Id == id);
            });
            return ph;
            
        }

        public Photo Create(Photo p)
        {
            context.Photos.Add(p);
            context.SaveChanges();
            return Fetch(p.Id);
        }

        public Photo Update(Photo p)
        {
            if (!DoesPhotoExist(p.Id))
                return null;

            context.Photos.Update(p);
            context.SaveChanges();
            RemoveFromCache(p.Id);
            return Fetch(p.Id);
        }

        public void Delete(int id)
        {
            if (!DoesPhotoExist(id))
                return;

            var p = new Photo { Id = id };
            context.Remove(p);
            RemoveFromCache(id);
            context.SaveChanges();
        }

        public bool DoesPhotoExist(int id)
        {
            return context.Photos.Any(p => p.Id == id);
        }

        private void RemoveFromCache(int id)
        {
            cache.TryGetValue<Photo>(id, out Photo p);
            if (null != p)
                cache.Remove(id);
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
