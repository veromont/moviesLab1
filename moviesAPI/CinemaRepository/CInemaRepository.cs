using moviesAPI.Models.CinemaContext;

namespace moviesAPI.Repositories
{
    public partial class CinemaRepository : IDisposable
    {
        private CinemaContext context;
        private bool disposed = false;

        public CinemaRepository(CinemaContext context)
        {
            this.context = context;
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
