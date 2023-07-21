using Microsoft.EntityFrameworkCore;
using moviesAPI.Models;
using moviesAPI.Models.CinemaContext;

namespace moviesAPI.Repositories
{
    /// <summary>
    ///  Provides access to the database via CRUD operations and some custom ones
    /// </summary>
    public partial class GenericCinemaRepository : IDisposable
    {
        private CinemaContext context;
        private bool disposed = false;
        public GenericCinemaRepository(CinemaContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            return await context.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity?> GetById<TEntity>(Guid id) where TEntity : class
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
        public async Task<TEntity?> GetById<TEntity>(int id) where TEntity : class
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
        public async Task<bool> Insert<TEntity>(TEntity entity) where TEntity : class
        {
            await context.Set<TEntity>().AddAsync(entity);
            return true;
        }
        public async Task<bool> Update<TEntity>(Guid id, TEntity entity) where TEntity : class
        {
            var modifiedEntity = await context.Set<TEntity>().FindAsync(id);
            if (modifiedEntity == null)
                return false;

            context.Entry(modifiedEntity).CurrentValues.SetValues(entity);
            context.Entry(entity).State = EntityState.Modified;
            return true;
        }
        public async Task<bool> Update<TEntity>(int id, TEntity entity) where TEntity : class
        {
            var modifiedEntity = await context.Set<TEntity>().FindAsync(id);
            if (modifiedEntity == null)
                return false;

            context.Entry(modifiedEntity).CurrentValues.SetValues(entity);
            context.Entry(entity).State = EntityState.Modified;
            return true;
        }
        public async Task<bool> Delete<TEntity>(Guid id) where TEntity : class
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return false;

            context.Set<TEntity>().Remove(entity);
            return true;
        }
        public async Task<bool> Delete<TEntity>(int id) where TEntity : class
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return false;

            context.Set<TEntity>().Remove(entity);
            return true;
        }
        public async Task<bool> EntityExists<TEntity>(Guid id) where TEntity : class
        {
            return await context.Set<TEntity>().FindAsync(id) != null;
        }
        public async Task<bool> EntityExists<TEntity>(int id) where TEntity : class
        {
            return await context.Set<TEntity>().FindAsync(id) != null;
        }
        public async Task<bool> Save()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }
        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
